using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : LivingEntity {

    [EnumFlags]
    public Species diet;

    public const int maxViewDistance = 10;

    public CreatureAction currentAction;
    public Genes genes;
    public Color maleColour;
    public Color femaleColour;

    // Visual settings:
    public float moveArcHeight = .2f;

    //  Constant Settings:
    public float timeBetweenActionChoices = 1;

    // Move data:
    public bool animatingMovement;
    public Coord moveFromCoord;
    public Coord moveTargetCoord;
    public Vector3 moveStartPos;
    public Vector3 moveTargetPos;
    public float moveTime;
    public float moveSpeedFactor;
    public float moveArcHeightFactor;
    public Coord[] path;
    public int pathIndex;

    public LivingEntity foodTarget;
    public Coord waterTarget;

    // Other
    public float lastActionChooseTime;
    public const float sqrtTwo = 1.4142f;
    public const float oneOverSqrtTwo = 1 / sqrtTwo;

    public virtual void FindFood () {
        LivingEntity foodSource = environment.SenseFood (coord, this, FoodPreferencePenalty);
        if (foodSource) {
            currentAction = CreatureAction.GoingToFood;
            foodTarget = foodSource;
            CreatePath (foodTarget.coord);

        } else {
            currentAction = CreatureAction.Exploring;
        }
    }

    public virtual void FindWater () {
        Coord waterTile = environment.SenseWater (coord);
        if (waterTile != Coord.invalid) {
            currentAction = CreatureAction.GoingToWater;
            waterTarget = waterTile;
            CreatePath (waterTarget);

        } else {
            currentAction = CreatureAction.Exploring;
        }
    }

    // When choosing from multiple food sources, the one with the lowest penalty will be selected
    public virtual int FoodPreferencePenalty (LivingEntity self, LivingEntity food) {
        return Coord.SqrDistance (self.coord, food.coord);
    }


    public void CreatePath (Coord target) {
        // Create new path if current is not already going to target
        /*if (path == null || pathIndex >= path.Length || (path[path.Length - 1] != target || path[pathIndex - 1] != moveTargetCoord)) {*/
            path = environment.environmentUtility.GetPath (coord.x, coord.y, target.x, target.y);
            pathIndex = 0;
        /*}*/
    }

    public void StartMoveToCoord (Coord target) {
        moveFromCoord = coord;
        moveTargetCoord = target;
        moveStartPos = transform.position;
        moveTargetPos = environment.tileCentres[moveTargetCoord.x, moveTargetCoord.y];
        animatingMovement = true;

        bool diagonalMove = Coord.SqrDistance (moveFromCoord, moveTargetCoord) > 1;
        moveArcHeightFactor = (diagonalMove) ? sqrtTwo : 1;
        moveSpeedFactor = (diagonalMove) ? oneOverSqrtTwo : 1;

        LookAt (moveTargetCoord);
    }

    public void LookAt (Coord target) {
        if (target != coord) {
            Coord offset = target - coord;
            transform.eulerAngles = Vector3.up * Mathf.Atan2 (offset.x, offset.y) * Mathf.Rad2Deg;
        }
    }

    public void OnDrawGizmosSelected () {
        if (Application.isPlaying) {
            var surroundings = environment.Sense (coord);
            Gizmos.color = Color.white;
            if (surroundings.nearestFoodSource != null) {
                Gizmos.DrawLine (transform.position, surroundings.nearestFoodSource.transform.position);
            }
            if (surroundings.nearestWaterTile != Coord.invalid) {
                Gizmos.DrawLine (transform.position, environment.tileCentres[surroundings.nearestWaterTile.x, surroundings.nearestWaterTile.y]);
            }

            if (currentAction == CreatureAction.GoingToFood) {
                var path = environment.environmentUtility.GetPath (coord.x, coord.y, foodTarget.coord.x, foodTarget.coord.y);
                Gizmos.color = Color.black;
                for (int i = 0; i < path.Length; i++) {
                    Gizmos.DrawSphere (environment.tileCentres[path[i].x, path[i].y], .2f);
                }
            }
        }
    }

}