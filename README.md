# hexania
Hexania is a Unity made world building game. The base block used for building here is <b>hex-block</b>.<br>
<h3>Controls</h3>
Use WASD or on-screen arrows to move around.
Swipe the screen to rotate and aim.
The game is meant for mobile-touch input.
<h3>What is going on here?</h3>
The most important thing here is the <b>HexChunk.cs</b> script which creates hex-based meshes on the fly.<br>
The idea is that there is a single mesh for a type of block (per chunk).
Also <b>AimHex.cs</b> script calculates the world-space coord of raycasted hit to coordinates of particular "hex".
Layout of hexes is 3 dimensional coords system with each odd row in z-axis shifted.

<img src="https://github.com/piotrklimko/hexania/blob/master/Screenshots/scr1.png">
