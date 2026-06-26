// --- BRUSH CONTROLS ---
const MAX_STROKE_WIDTH = 30.0; 
const MIN_STROKE_WIDTH = 10.5; 
const TEXTURE_GRIT = 140;     
const PRESSURE_WAVE = 2.5;    // Controls how smoothly the circle's thickness swells
const BASE_RADIUS = 140;      // Size of the circle

// --- SEED LOCKING SYSTEM ---
let activeSeed = null; 

function setup() {
  createCanvas(600, 480);
  
  if (activeSeed === null) {
    activeSeed = floor(random(1000000));
  }
  
  console.log("----------------------------------------");
  console.log("CURRENT CIRCLE SEED: " + activeSeed);
  console.log("----------------------------------------");
  
  noLoop(); 
}

function draw() {
  randomSeed(activeSeed);
  noiseSeed(activeSeed);

  background(28, 30, 33); // Dark slate chalkboard

  // Center of the canvas
  let cx = width / 2;
  let cy = height / 2;

  // Draw exactly ONE continuous chalk stroke in a perfect loop
  drawSingleFlatChalkCircle(cx, cy, BASE_RADIUS);
}

function mousePressed() {
  activeSeed = floor(random(1000000));
  setup(); 
  redraw();
}

/**
 * Draws a single chalk circle where coordinates scale outward along radius vectors
 */
function drawSingleFlatChalkCircle(centerX, centerY, radius) {
  // Determine step resolution based on circumference to maintain 1.5 samples per pixel
  let circumference = TWO_PI * radius;
  let steps = circumference * 1.5; 

  let noiseSeedY = random(10000);
  let noiseSeedX = random(10000);

  for (let i = 0; i <= steps; i++) {
    // Progress around the circle loop (from 0.0 to 1.0)
    let t = i / steps;
    
    // Convert progress to an angle in radians
    let angle = t * TWO_PI;

    // 1. Calculate the Perpendicular Normal Vector for a Circle
    // For circles, the normal direction points directly outward from the center
    let nx = cos(angle);
    let ny = sin(angle);

    // 2. Base straight pixel coordinate on the circumference perimeter
    let basePx = centerX + nx * radius;
    let basePy = centerY + ny * radius;

    // 3. Smooth Hand Pressure Matrix
    // CRITICAL: We pass angle to cos/sin or a looped space so the noise loops perfectly at the seam,
    // or keep a continuous 1D noise map. For glitching, standard 1D linear mapping works nicely.
    let pressureNoise = noise(noiseSeedY + t * PRESSURE_WAVE);
    if (pressureNoise < 0.15) continue; 

    // Smooth width variation
    let currentStrokeWidth = map(pressureNoise, 0.15, 1, MIN_STROKE_WIDTH, MAX_STROKE_WIDTH);

    // Subtle global hand wobble shifting the radius center
    let wobble = (noise(noiseSeedX + t * 3) - 0.5) * 2.5;

    // 4. Flat-Side Expansion along the radial vectors
    let fiberCount = 6; 
    for (let f = 0; f < fiberCount; f++) {
      let fiberT = f / (fiberCount - 1); 
      
      let grainNoise = noise(
        noiseSeedX + t * TEXTURE_GRIT, 
        noiseSeedY + fiberT * 12.0
      );

      let displacement = map(grainNoise, 0, 1, -currentStrokeWidth / 2, currentStrokeWidth / 2);

      // Final calculation pushing the point out or pulling it in relative to the radius center
      let finalX = basePx + nx * (wobble + displacement);
      let finalY = basePy + ny * (wobble + displacement);

      // Microscopic powder scattering
      finalX += random(-0.4, 0.4);
      finalY += random(-0.4, 0.4);

      let currentAlpha = map(pressureNoise, 0.15, 1, 30, 210);
      let currentWeight = map(pressureNoise, 0.15, 1, 0.6, 2.2);

      stroke(245, 245, 243, currentAlpha * random(0.6, 1.3));
      strokeWeight(currentWeight * random(0.7, 1.3));

      point(finalX, finalY);
    }
  }
}