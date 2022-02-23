# Spherical World and Biome Generation

#### Hello, this is an attempt at making a prcedurally generated sphreical world. The code in this project are not meant to be refactored and concise, as i no longer work on this branch. So please be aware of the unconventional practices and spaghetti-ness in this project, you have been warned! I am now working on a better version of this project could potentionally develop it into an procedurally generated sandbox/survival-ish game.


Here is a screenshot of what a fully generated world look like.

![An example of generated world](World.png)

---

I will briefly go over the procedures for generating a world like this

Regarding the mesh, what the program does is it generate a cube with six planes, each plane has a resolution, and each plane can be subdivided. If we normalize all the vectors on the cube, the verticies will form a sphere. To deal with a uneven distribution of the verticies on the sphere, a transformation is used.

![Cube World!!!](CubeWorld.png)


In order to make a culling of the generated mesh, each plane is divided into smaller subplanes. I define each subplane as a chunk of the mesh. In a coroutine, the chunks nearest the camera is appended into a queue alone with a callback. 

Each chunk in the queue is then processed over multiple threads. Each thread samples the height values through a height texture (which is not very precise), triangulate the verticies through marching square algorithm with multiple passes (which is not very efficient), and return the verticies and triangles alone with the callback.

Since the callback is in Unity's main thread, it takes the data and construct a mesh, and renders it. There is a boolean called "generate all". If true, the program will generate all the chunks over many threads and ignore the culling process. If false, the program will only generate nearby chunks. 

**If you want to "generate all", please make sure you set the plane resolution to no higher than 512 so that your computer doesn't explode**.

![With culling](Chunks.png)

---

Now i will briefly go over how to generate a similar biome

There are of course many different ways implement a procedurally generated biome system. I can generate a biome straight out of noise, or using rapid exploring random trees, or using control points. I created the biome through emergence. The idea is that we create a humidity map, and then a temperature map. Then we define the biome types according to those values.

<img src="diagram.jpeg" alt="drawing" width="400" margin-left="auto" marin-right="auto"/>