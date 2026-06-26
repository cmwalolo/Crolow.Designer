function setup() {
  createCanvas(600, 480);
  noLoop(); 
}

function draw() {
  background(30, 32, 35); // Dark chalkboard slate

  let rx = 120;
  let ry = 100;
  let rw = 360;
  let rh = 280;

  // Draw multiple overlapping chalk strokes using the normal-transpose method
  stroke(245, 245, 245);
  
  // 3-4 parallel tracks to get that layered multi-stroke chalk border look
  let tracks = [-6, 0, 6]; 
  
  for (let t = 0; t < tracks.length; t++) {
    let offset = tracks[t];
    let maxDist = random(3, 6); // Max displacement distance allowed for this track
    
    // Draw the 4 edges using the vector transpose function
    // We add an overshoot to the lengths so corners cross nicely
    let pad = 10; 
    
    // Top Edge (Left to Right)
    drawNormalChalkStroke(rx - pad, ry + offset, rx + rw + pad, ry + offset, maxDist);
    // Right Edge (Top to Bottom)
    drawNormalChalkStroke(rx + rw + offset, ry - pad, rx + rw + offset, ry + rh + pad, maxDist);
    // Bottom Edge (Right to Left)
    drawNormalChalkStroke(rx + rw + pad, ry + rh + offset, rx - pad, ry + rh + offset, maxDist);
    // Left Edge (Bottom to Top)
    drawNormalChalkStroke(rx + offset, ry + rh + pad, rx + offset, ry - pad, maxDist);
  }
}

/**
 * Draws a line by slicing it and transposing each pixel along its normal 
 * perpendicular vector using Perlin Noise capped by maxDistance.
 */
function drawNormalChalkStroke(x1, y1, x2, y2, maxDistance) {
  let distance = dist(x1, y1, x2, y2);
  let steps = distance; // Slice it finely (1 step per pixel)

  // Calculate the direction vector of the line
  let dx = x2 - x1;
  let dy = y2 - y1;
  
  // Calculate the perpendicular normal vector (rotated 90 degrees)
  // Normal of (dx, dy) is (-dy, dx)
  let nx = -dy / distance;
  let ny = dx / distance;

  // Generate unique random noise seeds for this specific stroke slice
  let noiseSeed = random(10000);

  // We loop twice per stroke to create a split, multi-grain chalk fiber look
  for (let pass = 0; pass < 2; pass++) {
    let passOffset = pass * 15.5; // Offset the noise space slightly for the second fiber
    
    // Lower alpha per fiber pass gives it that dusty, semi-transparent blend
    stroke(245, 245, 245, random(100, 180));
    strokeWeight(random(1, 2));

    beginShape();
    for (let i = 0; i <= steps; i++) {
      let t = i / steps;

      // 1. Calculate the base straight pixel position along the slice
      let basePx = lerp(x1, x2, t);
      let basePy = lerp(y1, y2, t);

      // 2. Use Perlin noise to calculate the scale factor (-1.0 to 1.0)
      // t * 40 sets a high frequency to get the gritty crumbly slate friction texture
      let noiseVal = noise(noiseSeed + passOffset + t * 40); 
      let nScale = map(noiseVal, 0, 1, -1, 1); 

      // 3. Transpose the pixel outwards along its normal vector based on maxDistance
      let finalX = basePx + nx * (nScale * maxDistance);
      let finalY = basePy + ny * (nScale * maxDistance);

      // Add a tiny bit of microscopic chalk grain powder noise
      finalX += random(-0.3, 0.3);
      finalY += random(-0.3, 0.3);

      // 4. Randomly break the stroke up to emulate porous rock leaving blank gaps
      if (random() > 0.12) {
        vertex(finalX, finalY);
      } else {
        endShape();
        beginShape();
      }
    }
    endShape();
  }
}