using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : Animal

{
    // Variable Settings
    float moveSpeed;
    float hungerResistance;
    float thirstResistance;
    float reproductiveCycle;

    float drinkQuantity;
    float foodQuantity;

    float criticalPercent = 0.7f;
    float visibility;
    float weight;
    

    // State:
    [Header("State")]
    public float hunger = 0;
    public float thirst = 0;
    public float reproduction = 0;
    public bool alreadyReproduced = false;

    public Bunny mateTarget;

    public override void Init(Coord coord, Environment env)
    {
        base.Init(coord, env);
        this.moveSpeed = env.moveSpeedBunny;
        this.hungerResistance = env.hungerResistanceBunny;
        this.thirstResistance = env.thirstResistanceBunny;
        this.reproductiveCycle = (this.hungerResistance * this.thirstResistance) / (this.hungerResistance + this.thirstResistance);
        moveFromCoord = coord;
        genes = Genes.RandomGenes(1);
        this.hunger = 0;
        this.thirst = 0;
        this.reproduction = 0;
        foodQuantity = 1 / this.hungerResistance;
        drinkQuantity = 1 / this.thirstResistance;
        this.alreadyReproduced = false;
        currentAction = CreatureAction.None;

        material.color = (genes.isMale) ? maleColour : femaleColour;
        this.gameObject.name = (genes.isMale) ? "Bunny(Male)" : "Bunny(Female)";

        ChooseNextAction();
    }



    protected virtual void FixedUpdate()
    {


        // Increase hunger and thirst over time
        hunger += Time.deltaTime * 1 / hungerResistance;
        thirst += Time.deltaTime * 1 / thirstResistance;
        if (reproduction > 1f)
        {

            if (alreadyReproduced == true)
            {
                GameObject.FindObjectOfType<Environment>().SpawnNewPopulations(this, coord);
            }
            reproduction = 0f;
            alreadyReproduced = false;
        }
        else
        {
            reproduction += Time.deltaTime * 1 / reproductiveCycle;
        }

        // Animate movement. After moving a single tile, the animal will be able to choose its next action
        if (animatingMovement)
        {
            AnimateMove();
        }
        else
        {
            // Handle interactions with external things, like food, water, mates
            HandleInteractions();
            float timeSinceLastActionChoice = Time.time - lastActionChooseTime;
            if (timeSinceLastActionChoice > timeBetweenActionChoices)
            {
                ChooseNextAction();
            }
        }

        if (hunger >= 1)
        {
            Die(CauseOfDeath.Hunger);
        }
        else if (thirst >= 1)
        {
            Die(CauseOfDeath.Thirst);
        }
    }

    // Animals choose their next action after each movement step (1 tile),
    // or, when not moving (e.g interacting with food etc), at a fixed time interval
    protected virtual void ChooseNextAction()
    {
        lastActionChooseTime = Time.time;
        // Get info about surroundings

        // Decide next action:
        // Eat if (more hungry than thirsty) or (currently eating and not critically thirsty)
        if (hunger >= criticalPercent)
        {
            FindFood();
        }
        // More thirsty than hungry
        else if (thirst >= criticalPercent)
        {
            FindWater();
        }

        else if (genes.isMale == true && currentAction != CreatureAction.Reproducing)
        {
            FindMate();
        }

        else if (genes.isMale == false && 0.25 < reproduction && reproduction < 0.5 && alreadyReproduced == false)
        {
            FindMate();
        }

        else
        {
            currentAction = CreatureAction.Exploring;
        }

        Act();

    }

    public void HandleInteractions()
    {
        if (currentAction == CreatureAction.Eating)
        {
            if (foodTarget && hunger > 0)
            {
                float eatAmount = hunger;
                eatAmount = foodTarget.Consume(eatAmount);
                hunger -= eatAmount;
            }
        }
        else if (currentAction == CreatureAction.Drinking)
        {
            if (thirst > 0)
            {
                float drinkAmount = thirst;
                thirst -= drinkAmount;
            }
        }
        else if (currentAction == CreatureAction.Reproducing)
        {
            if (genes.isMale == false && alreadyReproduced == false)
            {
                alreadyReproduced = true;
            }

        }
    }

    /*** Fin de série ***/
    public void AnimateMove()
    {
        // Move in an arc from start to end tile
        moveTime = Mathf.Min(1, moveTime + Time.deltaTime * moveSpeed * moveSpeedFactor);
        float height = (1 - 4 * (moveTime - .5f) * (moveTime - .5f)) * moveArcHeight * moveArcHeightFactor;
        transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, moveTime) + Vector3.up * height;

        // Finished moving
        if (moveTime >= 1)
        {
            environment.RegisterMove(this, coord, moveTargetCoord);
            coord = moveTargetCoord;

            animatingMovement = false;
            moveTime = 0;
            this.ChooseNextAction();
        }
    }

    public virtual void FindMate()
    {
        if (currentAction != CreatureAction.Reproducing)
        {
            if (mateTarget == null)
            {

                if (genes.isMale)
                {
                    List<Animal> mates = environment.SensePotentialMates(coord, this);
                    if (mates.Count >= 1)
                    {
                        mateTarget = (Bunny)mates[0];
                        currentAction = CreatureAction.GoingToMate;
                        CreatePath(mateTarget.coord);
                    }
                    else
                    {
                        currentAction = CreatureAction.SearchingForMate;
                    }
                }
                else
                {
                    currentAction = CreatureAction.SearchingForMate;
                }
            }
            else
            {
                currentAction = CreatureAction.GoingToMate;
                CreatePath(mateTarget.coord);
            }
        }

    }

    public void Act()
    {
        switch (currentAction)
        {
            case CreatureAction.Exploring:
                StartMoveToCoord(environment.GetNextTileWeighted(coord, moveFromCoord));
                break;
            case CreatureAction.GoingToFood:
                if (Coord.AreNeighbours(coord, foodTarget.coord))
                {
                    LookAt(foodTarget.coord);
                    currentAction = CreatureAction.Eating;
                }
                else
                {
                    StartMoveToCoord(path[pathIndex]);
                    pathIndex++;
                }
                break;
            case CreatureAction.GoingToWater:
                if (Coord.AreNeighbours(coord, waterTarget))
                {
                    LookAt(waterTarget);
                    currentAction = CreatureAction.Drinking;
                }
                else
                {
                    StartMoveToCoord(path[pathIndex]);
                    pathIndex++;
                }
                break;
            case CreatureAction.GoingToMate:
                if (Coord.AreNeighbours(coord, mateTarget.coord))
                {
                    if (mateTarget.alreadyReproduced == false)
                    {
                        LookAt(mateTarget.coord);
                        mateTarget.LookAt(this.coord);
                        mateTarget.currentAction = CreatureAction.Reproducing;
                        currentAction = CreatureAction.Reproducing;
                    }
                    else
                    {
                        StartMoveToCoord(environment.GetNextTileWeighted(coord, moveFromCoord));
                    }

                }
                else
                {
                    if (path == null)
                    {
                        mateTarget = null;
                        StartMoveToCoord(environment.GetNextTileWeighted(coord, moveFromCoord));
                    }
                    else
                    {
                        StartMoveToCoord(path[pathIndex]);
                        pathIndex++;
                    }

                }
                break;
            case CreatureAction.SearchingForMate:
                StartMoveToCoord(environment.GetNextTileWeighted(coord, moveFromCoord));
                break;
        }
    }

}
