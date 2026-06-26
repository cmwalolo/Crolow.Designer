// --- BRUSH CONTROLS ---
const MAX_STROKE_WIDTH = 30.0; 
const MIN_STROKE_WIDTH = 10.5; 
const TEXTURE_GRIT = 140;     
const PRESSURE_WAVE = 3.0;    

// --- HUMAN SKETCH CONTROLS (Tweak these!) ---
const SEGMENT_COUNT = 15;     // How many flat sides make up the circle shape
const OVERLAP_FACTOR = 1.25;  // Higher numbers make the strokes longer and overshoot past each other
const PASSES = 3;             // How many times the hand retraces the circle outline
const DRIFT_AMOUNT = 8.0;     // Maximum distance a stroke can accidentally drift off-target

let activeSeed = null; 

function setup() {
  createCanvas(600, 480);
  
  if (activeSeed === null) {
    activeSeed = floor(random(1000000));
  }
  
  console.log("----------------------------------------");
  console.log("CURRENT SKETCHY SEED: " + activeSeed);
  console.log("----------------------------------------");
  
  noLoop(); 
}

function draw() {
  randomSeed(activeSeed);
  noiseSeed(activeSeed);

  background(28, 30, 33); // Dark slate chalkboard

  let cx = width / 2;
  let cy = height / 2;
  let radius = 130;

  // Simulate a hand drawing the shape over multiple loose passes
  for (let p = 0; p < PASSES; p++) {
    
    // Each pass can have a slight intentional rotation or center shift
    let passRotation = random(-0.1, 0.1); 
    let passOffsetX = random(-DRIFT_AMOUNT/2, DRIFT_AMOUNT/2);
    let passOffsetY = random(-DRIFT_AMOUNT/2, DRIFT_AMOUNT/2);

    for (let i = 0; i < SEGMENT_COUNT; i++) {
      // Calculate the target angular sweep for this specific segment step
      let angle1 = (i / SEGMENT_COUNT) * TWO_PI + passRotation;
      let angle2 = ((i + 1) / SEGMENT_COUNT) * TWO_PI + passRotation;

      // Center angles used to calculate overshoots
      let midAngle = (angle1 + angle2) / 2;
      let angleDiff = angle2 - angle1;

      // Apply OVERLAP_FACTOR by widening the angular reach of this individual stroke
      let sketchAngle1 = midAngle - (angleDiff / 2) * OVERLAP_FACTOR;
      let sketchAngle2 = midAngle + (angleDiff / 2) * OVERLAP_FACTOR;

      // Add human tracking error (drift) to the radius at each point
      let r1 = radius + random(-DRIFT_AMOUNT, DRIFT_AMOUNT);
      let r2 = radius + random(-DRIFT_AMOUNT, DRIFT_AMOUNT);

      // Convert polar coordinates to Cartesian screen space (X, Y)
      let xStart = cx + passOffsetX + cos(sketchAngle1) * r1;
      let yStart = cy + passOffsetY + sin(sketchAngle1) * r1;
      let xEnd   = cx + passOffsetX + cos(sketchAngle2) * r2;
      let yEnd   = cy + passOffsetY + sin(sketchAngle2) * r2;

      // Draw this short segment as its own independent hand stroke
      drawSingleFlatChalkStroke(xStart, yStart, xEnd, yEnd);
    }
  }
}

function mousePressed() {
  activeSeed = floor(random(1000000));
  setup(); 
  redraw();
}

/**
 * Draws a single chalk stroke segment with Perlin grain texture
 */
function drawSingleFlatChalkStroke(x1, y1, x2, y2) {
  let distance = dist(x1, y1, x2, y2);
  let steps = distance * 1.2; 

  let dx = x2 - x1;
  let dy = y2 - y1;
  let nx = -dy / distance;
  let ny = dx / distance;

  let noiseSeedY = random(10000);
  let noiseSeedX = random(10000);

  for (let i = 0; i <= steps; i++) {
    let t = i / steps;

    let basePx = lerp(x1, x2, t);
    let basePy = lerp(y1, y2, t);

    let pressureNoise = noise(noiseSeedY + t * PRESSURE_WAVE);
    if (pressureNoise < 0.15) continue; 

    let currentStrokeWidth = map(pressureNoise, 0.15, 1, MIN_STROKE_WIDTH, MAX_STROKE_WIDTH);
    let wobble = (noise(noiseSeedX + t * 3) - 0.5) * 1.5;

    let fiberCount = 5; 
    for (let f = 0; f < fiberCount; f++) {
      let fiberT = f / (fiberCount - 1); 
      
      let grainNoise = noise(
        noiseSeedX + t * TEXTURE_GRIT, 
        noiseSeedY + fiberT * 12.0
      );

      let displacement = map(grainNoise, 0, 1, -currentStrokeWidth / 2, currentStrokeWidth / 2);

      let finalX = basePx + nx * (wobble + displacement);
      let finalY = basePy + ny * (wobble + displacement);

      finalX += random(-0.3, 0.3);
      finalY += random(-0.3, 0.3);

      let currentAlpha = map(pressureNoise, 0.15, 1, 30, 180);
      let currentWeight = map(pressureNoise, 0.15, 1, 0.5, 2.0);

      stroke(245, 245, 243, currentAlpha * random(0.6, 1.3));
      strokeWeight(currentWeight * random(0.7, 1.3));

      point(finalX, finalY);
    }
  }
}