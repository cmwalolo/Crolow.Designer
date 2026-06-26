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


// --- CRITICAL CONFIGURATION ---
const GRAIN_FREQUENCY = 120; // HIGHER = grittier, tighter chalk powder clusters
const PRESSURE_FREQUENCY = 6; // LOWER = longer, smoother shifts in hand pressure
const MAX_DISPLACEMENT = 4.5; // Width of the chalk stroke spread
const OVERSHOOT = 12;         // Corner overshoot length

function setup() {
  createCanvas(600, 480);
  noLoop(); 
}

function draw() {
  background(28, 30, 33); // Dark slate chalkboard

  let rx = 120;
  let ry = 100;
  let rw = 360;
  let rh = 280;

  // Let's do 3 intersecting passes per side
  let passes = 3; 
  
  for (let p = 0; p < passes; p++) {
    // Randomize offsets per pass so lines weave and cross over one another
    let offTop    = random(-6, 6);
    let offBottom = random(-6, 6);
    let offLeft   = random(-6, 6);
    let offRight  = random(-6, 6);
    let pad       = OVERSHOOT + random(-4, 4);

    // Top Side
    drawGrainyPressureStroke(rx - pad, ry + offTop, rx + rw + pad, ry + offTop);
    // Right Side
    drawGrainyPressureStroke(rx + rw + offRight, ry - pad, rx + rw + offRight, ry + rh + pad);
    // Bottom Side
    drawGrainyPressureStroke(rx + rw + pad, ry + rh + offBottom, rx - pad, ry + rh + offBottom);
    // Left Side
    drawGrainyPressureStroke(rx + offLeft, ry + rh + pad, rx + offLeft, ry - pad);
  }
}

/**
 * Renders a chalk stroke using microscopic points. Noise maps out both the 
 * perpendicular transposition, hand pressure thickness, and chalk grain breakups.
 */
function drawGrainyPressureStroke(x1, y1, x2, y2) {
  let distance = dist(x1, y1, x2, y2);
  let steps = distance * 2; // Sample twice per pixel for rich, dense powder packing

  let dx = x2 - x1;
  let dy = y2 - y1;
  let nx = -dy / distance;
  let ny = dx / distance;

  // Independent noise tracks for structure, pressure, and granularity
  let seedStructure = random(50000);
  let seedPressure  = random(50000);
  let seedGrain     = random(50000);

  for (let i = 0; i <= steps; i++) {
    let t = i / steps;

    // 1. Base linear trajectory
    let basePx = lerp(x1, x2, t);
    let basePy = lerp(y1, y2, t);

    // 2. Hand Pressure (Low frequency noise determines line fading/thickness)
    let pressureNoise = noise(seedPressure + t * PRESSURE_FREQUENCY);
    
    // Skip rendering entirely if hand pressure drops too low (porous stone gaps)
    if (pressureNoise < 0.22) continue; 

    // Scale weight and opacity proportionally to the hand pressure
    let currentWeight = map(pressureNoise, 0.22, 1, 0.5, 3.2);
    let currentAlpha  = map(pressureNoise, 0.22, 1, 40, 230);

    // 3. Micro Texture Grain (High frequency noise controls fine grit displacement)
    let structureNoise = noise(seedStructure + t * 25); // Gentle wobble shape
    let grainNoise     = noise(seedGrain + t * GRAIN_FREQUENCY); // Rough texture shape
    
    // Combine macro shape with microscopic grit
    let combinedScale = map(structureNoise, 0, 1, -0.6, 0.6) + map(grainNoise, 0, 1, -0.4, 0.4);

    // 4. Perpendicular Transposition Calculation
    let finalX = basePx + nx * (combinedScale * MAX_DISPLACEMENT);
    let finalY = basePy + ny * (combinedScale * MAX_DISPLACEMENT);

    // Microscopic scattering explosion mimicking crumbling chalk powder dust
    finalX += random(-0.5, 0.5);
    finalY += random(-0.5, 0.5);

    // Apply custom pressure values dynamically to this specific slice coordinate
    stroke(245, 245, 243, currentAlpha * random(0.7, 1.2));
    strokeWeight(currentWeight * random(0.8, 1.3));

    // CRITICAL: Draw as individual particles instead of connected vector points
    point(finalX, finalY);
  }
}


// --- BRUSH CONTROLS ---
const STROKE_WIDTH = 30;     // Maximum thickness of the single flat stroke
const TEXTURE_GRIT = 300;     // HIGHER = tighter, crispier chalk grain clusters
const PRESSURE_WAVE = 2;      // LOWER = longer, smoother transitions in hand pressure
const OVERSHOOT = 12;         // Corner extension length

function setup() {
  createCanvas(600, 480);
  noLoop(); 
}

function draw() {
  background(28, 30, 33); // Dark slate chalkboard

  let rx = 120;
  let ry = 100;
  let rw = 360;
  let rh = 280;

  // Draw the frame - exactly ONE stroke pass per side
  let pad = OVERSHOOT;

  // Top Side
  drawSingleFlatChalkStroke(rx - pad, ry, rx + rw + pad, ry);
  // Right Side
  drawSingleFlatChalkStroke(rx + rw, ry - pad, rx + rw, ry + rh + pad);
  // Bottom Side
  drawSingleFlatChalkStroke(rx + rw + pad, ry + rh, rx - pad, ry + rh);
  // Left Side
  drawSingleFlatChalkStroke(rx, ry + rh + pad, rx, ry - pad);
}

/**
 * Draws a single, multi-fiber chalk stroke using 2D noise space matrixing
 */
function drawSingleFlatChalkStroke(x1, y1, x2, y2) {
  let distance = dist(x1, y1, x2, y2);
  let steps = distance * 1.5; // Fine step resolution along the path

  let dx = x2 - x1;
  let dy = y2 - y1;
  let nx = -dy / distance;
  let ny = dx / distance;

  // Unique noise coordinates for this specific stroke
  let noiseSeedY = random(10000);
  let noiseSeedX = random(10000);

  // Travel step-by-step down the single line length
  for (let i = 0; i <= steps; i++) {
    let t = i / steps;

    // Base straight line coordinate
    let basePx = lerp(x1, x2, t);
    let basePy = lerp(y1, y2, t);

    // 1. Hand Pressure: Evaluated once per step down the line
    let pressureNoise = noise(noiseSeedY + t * PRESSURE_WAVE);
    if (pressureNoise < 0.2) continue; // Porous stone skipping effect

    // 2. Structural Wobble: Gentle hand-shakiness shifting the entire stroke center
    let wobble = (noise(noiseSeedX + t * 4) - 0.5) * 3.5;

    // 3. Flat-Side Expansion: Draw multiple sub-fibers across the normal vector
    // This creates the internal strands and thickness in a single pass
    let fiberCount = 6; 
    for (let f = 0; f < fiberCount; f++) {
      let fiberT = f / (fiberCount - 1); // Normalize across brush width (0.0 to 1.0)
      
      // Calculate 2D Noise value using both line progress (t) AND fiber position (fiberT)
      // This forces internal grains to weave, split, and cross over each other organically
      let grainNoise = noise(
        noiseSeedX + t * TEXTURE_GRIT, 
        noiseSeedY + fiberT * 12.0
      );

      // Map grain noise to left/right displacement boundaries
      let displacement = map(grainNoise, 0, 1, -STROKE_WIDTH / 2, STROKE_WIDTH / 2);

      // Final position: Base Line + Hand Wobble + Perpendicular Fiber Displacement
      let finalX = basePx + nx * (wobble + displacement);
      let finalY = basePy + ny * (wobble + displacement);

      // Add a tiny bit of random powder spray scattering
      finalX += random(-0.4, 0.4);
      finalY += random(-0.4, 0.4);

      // Dynamically modify stroke profile based on the pressure noise
      let currentAlpha = map(pressureNoise, 0.2, 1, 30, 190);
      let currentWeight = map(pressureNoise, 0.2, 1, 0.6, 2.2);

      // Apply style attributes per grain particle
      stroke(245, 245, 243, currentAlpha * random(0.6, 1.3));
      strokeWeight(currentWeight * random(0.7, 1.3));

      // Plot the individual dust flake
      point(finalX, finalY);
    }
  }
}