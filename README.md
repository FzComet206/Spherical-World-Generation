# Spherical World and Biome Generation

#### Hello, this is an attempt at making a prcedurally generated sphreical world. The code in this project are not meant to be refactored and concise, as i no longer work on this branch. So please be aware of the unconventional practices and spaghetti-ness in this project. You have been warned! I am now working on a better version of this project and could potentionally develop it into an procedurally generated sandbox/survival-ish game.


Here is a screenshot of what a fully generated world look like.

![An example of generated world](Imgs/World.png)

---

I will briefly go over the procedures for generating a world like this

Regarding the mesh, what the program does is it generate a cube with six planes, each plane has a resolution, and each plane can be subdivided. If we normalize all the vectors on the cube, the verticies will form a sphere. To deal with a uneven distribution of the verticies on the sphere, a transformation is used.

![Cube World!!!](Imgs/CubeWorld.png)

---

In order to make a culling of the generated mesh, each plane is divided into smaller subplanes. I define each subplane as a chunk of the mesh. In a coroutine, the chunks nearest the camera is appended into a queue alone with a callback. 

Each chunk in the queue is then processed over multiple threads. Each thread samples the height values through a height texture (which is not very precise), triangulate the verticies through marching square algorithm with multiple passes (which is not very efficient), and return the verticies and triangles alone with the callback.

Since the callback is in Unity's main thread, it takes the data and construct a mesh, and renders it. There is a boolean called "generate all". If true, the program will generate all the chunks over many threads and ignore the culling process. If false, the program will only generate nearby chunks. 

**If you want to "generate all", please make sure you set the plane resolution to no higher than 512 so that your computer doesn't explode**.

<p align="center"> <img src="Imgs/Chunks.gif" alt="Chunks" width="800"/> </>

---

Now i will briefly go over how to generate a similar biome

Note that all textures and noises below are the results of a compute shader in unity, in order to increse the proformance.

---

There are of course many different ways implement a procedurally generated biome system. I can generate a biome straight out of noise, or using rapid exploring random trees, or using biome control points. I created the biome through the concept of "emergence". The idea is that we create a humidity map, and then a temperature map. Then we define the biome types according to those values.

<p align="center"> <img src="Imgs/diagram.jpeg" alt="diagram" width="800"/> </>

---

To construct a humidity map and a temperature map, i first constructed following textures:

- continent map
- altitude map
- proximity map
- height map
- temperature map
- humidity map

The first thing is to determine which point is ocean and which point is ocean and which point is land. In order to do that, i wrote program that randomly generated a continent map texture. Here are the steps:

- remap the index of each texture into a altitude and a longitude, then conver the coordinate to vector on a sphere.

- use that vector as the position of the simplex noise sample. The simplex noise also uses fractal brownian motion.

- set a threashold for the noise values, the ones above the threashold is continent, and the ones below the threshold is ocean.

- repeat the above process one more time with a smaller noise scale, then layer the two noises together

Here is an example continent map.

![Continent](Imgs/Continent.png)

---

Now generate a altitude map. The value at each point on the altitude map is generated by how far the pixel is to the top and bottom of the texture. So the center of the texture has values close to 1, and the top/has the values close to 0.

Then some turbulence alone the y axis is applied to the texture, in order to add randomness to the altitude map. This is useful so that every altitude map is different.

Here is the output

![Altitude](Imgs/Altitude.png)

---

Next up the the proximity map. I define a proximity map as to how close a point is to the ocean. I did not write an algorithm exactly for that becase that will be really slow. What i did is to just retain the noise values when generating the continent map, remap the values to a ranges like 0.6 to 1, and multiply the noises with the continent map. Naturally, the point with higher values was farther from the ocean. 

Here is the output

![Proximity](Imgs/Proximity.png)

---

Next up is a fairly simple one. I defined the height map to just represent the elevations of each point. Here is the output.

![Height map](Imgs/Elevation.png)

---

Now we finished construcing all the necessary input maps. Let's take a look at temperature map.

Here is the basic idea:

- foreach pixel, sample the corresponding pixel in the altitude map. Higher values (close to equator) means higher temperature, vice versa.

- foreach pixel, sample the corresponding pixel in the proximity map. Higher values (far from ocean) means the temperature skew to both ends (0.8 is like 0.9, and 0.3 is like 0.1)

- foreach pixel, sample the corresponding pixel in the height map. Higher values (high elevation) means lower temperature.

- adjust the specific weights for each samples and add them together, thus form a temperature map. 

Here is the output

![Temperature](Imgs/Temperature.png)

---

And lastly the humidity map using similar methods above:

- higher altitude means lower humidity

- higher proximity means lower humidity

- higher elevation means lower humidity

Here is the output

![Humidity](Imgs/Humidity.png)

---

Now we have a humidity map and a temperature map, we can specify biomes according to each biome's humidity and temperature threshold. In unity, i wrote custom function in shader graph that took all the biome threshold and colors as input, and output the biome color depending on the temperature and humidity values. This draws biomes represented by colors on the sphere.

To make the shader precise and consistent, i changed the encoding of the two textures to R16 instead of 8 bits. The temperature and humidity maps can be linearly interpolated in the shader, thus making the biome boundary more smooth instead of pixalated.


Here is an example of a biome that is supposed to be desert (orange) surrounded by other biomes (black)

![Desert](Imgs/Desert.png)

---

The material uses a shader that takes temperature and humidity threshold values and color of each biome as input, and renders it on the sphere. These boundaries can be adjusted during runtime as shown in the gif.

<p align="center"> <img src="Imgs/Thresholds.gif" alt="Boundaries" width="800"/> </>

---

**if you deleted the textures in "generated" folder, they will be regenerated when you click on play button, but make sure to change the texture formate of humidity and temperature map to R16 for the correct biome boundaries.**

Next up are the height values at each vertex.

In this project's implementation of mesh gerenration algorithm, I use height map based approach for encoding terrains. There are downside to this approach, such as the lack of overhangs and vertical cliffs. However, since the mesh generation algorithm i used in this project is marching squares. I used some tricks to generate a vertical wall for some configurations of marching squares.

To get the height values at each pixel, we first generate a M x N texture through compute shader, with each pixel's index remapped into longgitude and latitude. The coordinate is then transformed to positions on sphere as float3 (or vector 3). The position is then used to sample height values from a simplex noise library and fractsal brownian motions.

Here is the generated height map

![Height Map](Imgs/HeightMap.png)

---

After the generation of heightmap is complete, the height map texture is then read into memory as a nested array. Then the array is accessed through threads. The value of the vertex position woule be something like finalVert = originalVert * (1 + height_scalar * height) where height scalar is a very small number like 0.001, and height is the corresponding noise value.

There is one major problem with this approach. Since the heigh values at each vertex on a sphere is read from a rectangular texture, there are distortions as the verticies does not have a one to one mapping to texture indexes. 

There are two ways that i think of can solve this problem:

- Move the noise calculations from gpu to cpu with multithreading, which could make things slower. But as a result, the noise values at each vertex would be accurate.

- Use a large compute buffer to represent verticies on the sphere, and return the values back to cpu.

The latter approach is better in practice. Even though i have not yet implemented this approach to this project, i have decided to use this approach in the project i'm currently working on.

One thing that i would like to mention is that, since i found a way to define biome boundaries, there is also a way to apply different height curves and blend different height curves between the biomes. For example, the shrubland biomes can on average have a very high higher values, while the desert biome next to it can on average have a very low value. 

The way i do it is to describe different biome's height curves as an animation curve. Where the first t axis is the input noise values, and the y axis is the height values given the actual noise value. After defining all the curves. I pre-sample all the curves in cpu code and combine it into one compute buffer, then generate height values according to the compute buffer in gpu side.

There can also be blendings in between biomes, so that there is a smooth height and the values don't just have some sort of wierd discontinuity. In order to blend height values across biomes, i will have to blend them in humidity-temperature map space, since that is how the boimes are defines. For example, the points with humidity value close to one boundary, we can set the height to be equal to some portion of the current biome plus some portion of the neighboring biome. I did not implement this in this project yet, and i will do this the my upcoming project. 

Here is an example of what a biome curve would look like.

![Biome Curve](Imgs/Editor.png)

---

We have discussed the methods for generating verticies on sphere, use multi-threading to improve proformance, use the "emergence" method to generate a natural-looking biome. Finally, i used marching squraes to define the triangles and verticies. I used marching squares because i wanted to have smoother edges and vertical walls. It might not be the best way to generate meshes, but i gave it a try and it looks ok. The better way of triangulating the meshes through this algorithm might be to use a triangulation table and do the computation in parallel with compute shader.


Here is an image of the marching square configurations

![Squares](Imgs/squares.png)

---

And here are two images of marching square configurations with wireframe on

![Wireframe](Imgs/Wireframe.png)

---

![Wireframe](Imgs/Wireframe2.png)

---

Finally we have the "World". There are different seeds in the inspector, you can change them to have a different world.

![SimpleWorld](Imgs/SimpleWorld.png)

---

#### There could be an ocean shader, some atmosphere shader, better lighting, and maybe a better planet with marching cubes algorithm "with overhang mountains and ocean terrains), along with a lot better implementation and less spaghetti. And that is what i am working on next. I plan to build the next project into a actual program that can be run on windows and mac machines, and it would be like a very simple sandbox or survival-ish game, with a heavy focus on environment and procedural contents. The player would just swing arond the globe with a hook gun and explore.

#### My inspiration come from the youtuber named Sabastian Lague, and i used a little bit of his code, go check out his channel!

---

Feel free to add me on discord!

![](https://dcbadge.vercel.app/api/shield/280385777664524288)


