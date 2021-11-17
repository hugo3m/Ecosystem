# Ecosystem

Init public repo from Sebastian Lague https://github.com/SebLague/Ecosystem-2/tree/master
The Ecosystem Simulation video is available here https://youtu.be/r_It_X7v-1E

## Explanation of the ecosystem

The Ecosystem is composed of - Fox - Rabbit - Plants

The plants grow at regular intervals at random positions on the ground.
The foxes eat the rabbits and the rabbits eat the plants. The foxes and rabbits can breed.
The female carries the baby in her womb for some time before giving birth.
Rabbits and foxes need to feed and drink to avoid dying of hunger and thirst.

## Implementation of genetic algorithm

Rabbits and foxes need to feed and drink to avoid dying of hunger and thirst. The problem is that if the foxes eat the rabbits too quickly, they doom themselves by eliminating their only food source. Similarly, if the rabbits eat the grass too quickly. The aim is to find the optimal parameters for the world to survive as long as possible. For this purpose a genetic algorithm is implemented. It tries out values for the different parameters and then checks whether the neighbours have better results than the current values. If this is the case, the current values take the values of the best neighbour.

## Run the project

The project is done in Unity. You need a recent version of Unity to run the project. If you have any problems or would like more information about the code, please contact me.
