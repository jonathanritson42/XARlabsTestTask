# XARlabsTestTask

I have split each stage into it's own scene to show my progression and to quickly and easily switch between them without the need to use source control for this task. However in a real project I would only keep the most recent and up to date version of a script and scene due to it being recorded in source control anyway.

Project setup

// To be completed


Initial Ideas

Unity Project setup - 2023.3 doesn't exist (other than beta versions which can be useful but can also be less stable) - used unity 6 LTS instead with URP


Task 1 Process

Initially setup creating the object with the correct components. 
Then make sure this is working by entering play mode.
Start by generating a sphere - By generating the Verts first, then joining them together with Tris.
Do the same with creating the cone, remembering the offset from the base sphere.
Combine the two parts of the mesh togther.
Calculate normals to get correct lighting
Create the material and specify to use the URP shader.
Test end result by entering play mode.


Task 2 Process

Created new scene.
Duplicated existing mesh creation.
Realised that a base class would be benificial as both mesh generation use similar code.
Added colour and position values to newly generated object.
Test end result by entering play mode.


Task 3 Process

Using the previous scene as a base create a Lissajous animation implmentation.
Implment the base of the formula, to be able to get coordinates that are able to apply positions.
Add this to the object positioning with starting offset if needed.
Update positioning reletive to time.
Add a way to randomise values using an enum.
Add animation controll to mesh generation base state.
Test end result by entering play mode.
- Just call RandomLissajouseAnimation() at runtime if an update to the values is needed.
- Or in editor use the context menu to randomise animation values.


Task 4 Process

Update just Object A generator to allow for rotation to look at object B.
Add follow Object selection in inspector and use Vector3 for angle speed control for greater accuracy.
Initially set up rotate towards using transform.position rather than the generated object and needed to change to generatedObject.transform.position.
Used MoveTowardsAngle as it's smoother and easily allows for finer controls over specific speeds.


Task 5 Process

Added colour change to happen before rotation to allow to slightly better optimise the update loop.
