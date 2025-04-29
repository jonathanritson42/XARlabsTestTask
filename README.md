# XARlabsTestTask

I have split each stage into it's own scene to show my progression and to quickly and easily switch between them without the need to use source control for this task. However in a real project I would only keep the most recent and up to date version of a script and scene due to it being recorded in source control anyway

I also wouldn't usually include builds in a git repo but for convenience and ease I have also included it here.


--Project setup--

Using Unity 6
Open each test scene and enter playmode to demonstrate each task
Alturnatvely change git history to systematically apply each task

For bonus build either install provided android apk (/Builds/...) to an android phone that is at least android version 12 and ARCore supported or Switch profile to the preset Android profile (File/BuildProfiles), make sure the XR plug-in is using the correct setting (Android/Google ARCore - AR) and Build to device.


--Initial Ideas--

Unity Project setup - 2023.3 doesn't exist (other than beta versions which can be useful but can also be less stable) - used unity 6 LTS instead with URP


--Task 1 Process--

Initially setup creating the object with the correct components
Then make sure this is working by entering play mode
Start by generating a sphere - By generating the Verts first, then joining them together with Tris
Do the same with creating the cone, remembering the offset from the base sphere
Combine the two parts of the mesh togther
Calculate normals to get correct lighting
Create the material and specify to use the URP shader
Test end result by entering play mode


--Task 2 Process--

Created new scene
Duplicated existing mesh creation
Realised that a base class would be benificial as both mesh generation use similar code
Added colour and position values to newly generated object
Test end result by entering play mode


--Task 3 Process--

Using the previous scene as a base create a Lissajous animation implmentation
Implment the base of the formula, to be able to get coordinates that are able to apply positions
Add this to the object positioning with starting offset if needed
Update positioning reletive to time
Add a way to randomise values using an enum
Add animation controll to mesh generation base state
Test end result by entering play mode
- Just call RandomLissajouseAnimation() at runtime if an update to the values is needed
- Or in editor use the context menu to randomise animation values


--Task 4 Process--

Update just Object A generator to allow for rotation to look at object B
Add follow Object selection in inspector and use Vector3 for angle speed control for greater accuracy
Initially set up rotate towards using transform.position rather than the generated object and needed to change to generatedObject.transform.position
Used MoveTowardsAngle as it's smoother and easily allows for finer controls over specific speeds


--Task 5 Process--

Added colour change to happen before rotation to allow to slightly better optimise the update loop
Found the correect direction from Object B from Object A
Normalsed the value to get the direction
Used Dot product to find out if in front or behind
Set colour depending on positive or negative


--Task 6 Process--

Setup new mesh that clones original
For each vertices use Perlin Noise to get the ammount of noise by specified value, then displace it
Apply this to each vertices 
Then apply this to the cloned mesh
Recalculate bounds to make sure the bounding volume is correct

--Bonus--

AR

Import packages
Create AR scene 
Setup XR Origin including Plane manager and RaycastManager
Create Script to send and detect raycast hits on ARPlanes
Update script to be able to spawn and scale Object A & B.