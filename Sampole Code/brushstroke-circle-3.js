// --- BRUSH CONTROLS ---
const MAX_STROKE_WIDTH = 30.0; 
const MIN_STROKE_WIDTH = 15.5; 
const TEXTURE_GRIT = 140;     
const PRESSURE_WAVE = 3.0;    

// --- HUMAN SKETCH CONTROLS ---
const SEGMENT_COUNT = 15;      // Draw it with 5 loose strokes (try 3, 5, or 8!)
const OVERLAP_FACTOR = 1.25;  // Extension padding causing strokes to overshoot
const PASSES = 3;             // Overlapping sketching passes
const DRIFT_AMOUNT = 8.0;     // Radial drift away from the perfect perimeter

let activeSeed = null; 

function setup() {
  createCanvas(600, 480);
  
  if (activeSeed === null) {
    activeSeed = floor(random(1000000));
  }
  
  console.log("----------------------------------------");
  console.log("CURRENT CURVED SKETCH SEED: " + activeSeed);
  console.log("----------------------------------------");
  
  noLoop(); 
}

function draw() {
  randomSeed(activeSeed);
  noiseSeed(activeSeed);

  background(28, 30, 33); // Dark chalkboard

  let cx = width / 2;
  let cy = height / 2;
  let radius = 130;

  for (let p = 0; p < PASSES; p++) {
    // Each pass gets a minor overall tracking drift shift
    let passRotation = random(-0.15, 0.15); 
    let passOffsetX = random(-DRIFT_AMOUNT/2, DRIFT_AMOUNT/2);
    let passOffsetY = random(-DRIFT_AMOUNT/2, DRIFT_AMOUNT/2);

    for (let i = 0; i < SEGMENT_COUNT; i++) {
      // Base segment angles
      let angle1 = (i / SEGMENT_COUNT) * TWO_PI + passRotation;
      let angle2 = ((i + 1) / SEGMENT_COUNT) * TWO_PI + passRotation;

      let midAngle = (angle1 + angle2) / 2;
      let angleDiff = angle2 - angle1;

      // Extend angles so strokes overshoot each other along the arc
      let sketchAngle1 = midAngle - (angleDiff / 2) * OVERLAP_FACTOR;
      let sketchAngle2 = midAngle + (angleDiff / 2) * OVERLAP_FACTOR;

      // Each endpoint has a minor radial inaccuracy
      let r1 = radius + random(-DRIFT_AMOUNT, DRIFT_AMOUNT);
      let r2 = radius + random(-DRIFT_AMOUNT, DRIFT_AMOUNT);

      // Draw the stroke as an ARC rather than a straight line
      drawCurvedChalkSegment(cx + passOffsetX, cy + passOffsetY, sketchAngle1, sketchAngle2, r1, r2);
    }
  }
}

function mousePressed() {
  activeSeed = floor(random(1000000));
  setup(); 
  redraw();
}

/**
 * Renders a single chalk stroke that curves smoothly along an angular sweep
 */
function drawCurvedChalkSegment(cx, cy, startAngle, endAngle, startRadius, endRadius) {
  // Approximate distance along this arc segment to scale step counts
  let avgRadius = (startRadius + endRadius) / 2;
  let arcLength = abs(endAngle - startAngle) * avgRadius;
  let steps = arcLength * 1.2; 

  let noiseSeedY = random(10000);
  let noiseSeedX = random(10000);

  for (let i = 0; i <= steps; i++) {
    let t = i / steps;

    // 1. Calculate the curved base path using polar coordinates
    let currentAngle = lerp(startAngle, endAngle, t);
    let currentRadius = lerp(startRadius, endRadius, t);

    // 2. The perpendicular normal vector points directly OUTWARD from the circle center
    let nx = cos(currentAngle);
    let ny = sin(currentAngle);

    // Coordinate on the mathematically curved base perimeter
    let basePx = cx + nx * currentRadius;
    let basePy = cy + ny * currentRadius;

    // 3. Noise & Pressure Matrix
    let pressureNoise = noise(noiseSeedY + t * PRESSURE_WAVE);
    if (pressureNoise < 0.15) continue; 

    let currentStrokeWidth = map(pressureNoise, 0.15, 1, MIN_STROKE_WIDTH, MAX_STROKE_WIDTH);
    let wobble = (noise(noiseSeedX + t * 3) - 0.5) * 1.5;

    // 4. Flat brush texture expansion along the radial normal vectors
    let fiberCount = 5; 
    for (let f = 0; f < fiberCount; f++) {
      let fiberT = f / (fiberCount - 1); 
      
      let grainNoise = noise(
        noiseSeedX + t * TEXTURE_GRIT, 
        noiseSeedY + fiberT * 12.0
      );

      let displacement = map(grainNoise, 0, 1, -currentStrokeWidth / 2, currentStrokeWidth / 2);

      // Displace outward/inward along the normal vector relative to the arc curve
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