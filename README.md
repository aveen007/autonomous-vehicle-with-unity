# autonomous-vehicle-with-unity
An autonomous vehicle with unity simulator.
AI code section is not provided please email me at aveen2000hussein@gmail.com for access




## Documentation

[Documentation](https://github.com/aveen007/autonomous-vehicle-with-unity/blob/main/Aveen%20Hussein2.pdf)
view full project pdf for more details

# Simulation
This project is a simulator of two training environments for a self-driving car using ml-agents.
From the simulator side, a city is created with a custom car physics based on simulating 6 forces acting on a car which are:
- suspension
- acceleration
- steering
- brakes
- slipping
- friction
![image](https://github.com/aveen007/autonomous-vehicle-with-unity/assets/73739296/fa9a7b1c-31a9-4608-a213-1e3d17ae0aba)

Animation curves were used for a hyperrealistic feel and extra control over the car.

The city simulation was enhanced with a traffic system that was implemented through a waypoint tool created to easily add and remove waypoints and branching at road crossings to increase randomness as well having different directions and speeds of the traffic objects.
- traffic objects are spawned using object pooling and a navigation mesh.
- the implemented traffic system is used to spawn and conrtol pedestrians and other cars into the scene.

- a checkpoint system is also implemented in the scene to help with the learning algorithm.
- sensors used for the simulation are LIDAR and Camera
# Python API
- Unity ml-agents release 20 is used in this project.
- Custom algorithm is implemented using Python Low Level API (PLL API).
- Two custom side channels are implemented to send and recieve data for UI visualization.
- Standalone builds are used for different operating systems with the PLL API so no installation is required.
# Learning algorithm
- Camera data is processed for Lane detection and Object detection
- CLRNet algorithm is used for Lane detection
- YOLOv8 is used for object detection
- DQN algorithm is used for car control (throttle/steering)
- Two models tested and results compared for LIDAR data and LIDAR data inhanced with specially processed CLRNet data from camera images.
- Reward function suggested and tested.
- Learning from demonstrations is implemented through adding experiences to the replay memory, architecture of the network shown below

![image](https://github.com/aveen007/autonomous-vehicle-with-unity/assets/73739296/2da26b10-0a68-4f64-8cb4-1d5c9e8ab22c)

# UI and Custom side channels

- A visualization tool was created in the unity side to view CLRNet lanes and YOLOv8 objects in real time.
- Car speed and steer are visualized in the UI as well as a minimap from the observer POV.
- Car speed and steer and both the cost and reward are sent from unity to the PLL API for easy training and tracking in tensorboard.

# Demo
You can view the city at <div align="center">
  <a href="https://www.youtube.com/watch?v=dLL61kv3VtI"><img src="https://www.youtube.com/watch?v=dLL61kv3VtI/0.jpg" alt="car"></a>
</div>
UI for YOLOv8 in unity  <div align="center">
  <a href="https://www.youtube.com/watch?v=dLL61kv3VtI"><img src="https://www.youtube.com/watch?v=dLL61kv3VtI/0.jpg" alt="car"></a>
</div>
CLRNet in unity  <div align="center">
  <a href="https://www.youtube.com/watch?v=jcJKDtwVbVY"><img src="https://www.youtube.com/watch?v=jcJKDtwVbVY/0.jpg" alt="car"></a>
</div>
DQN algorithm for LIDAR data with Image data from CLRNet <div align="center">
  <a href="https://www.youtube.com/watch?v=nSKgw1XCndI1"><img src="https://www.youtube.com/watch?v=nSKgw1XCndI1/0.jpg" alt="car"></a>
</div> 


## Acknowledgements

 - [CLRNet lane detection](https://github.com/Turoad/CLRNet)
 - [YOLOv8 object detection](https://github.com/ultralytics/ultralytics.git)
 - [Unity ML-Agents](https://github.com/Unity-Technologies/ml-agentst)

