const MAX_STROKE_WIDTH = 9.0; 
const MIN_STROKE_WIDTH = 2.5; // Raised slightly to keep width changes gentle and smooth
const TEXTURE_GRIT = 140;     
const PRESSURE_WAVE = 2.5;    // Lowered from 4 to 2.5 for much smoother, gradual width transitions
const OVERSHOOT = 12;         

// --- SEED LOCKING SYSTEM ---
// Paste a saved seed number here to lock it, or keep it at null to randomize on click
let activeSeed = null; 

function setup() {
  createCanvas(600, 480);
  
  // Choose a new random seed if one isn't locked in yet
  if (activeSeed === null) {
    activeSeed = floor(random(1000000));
  }
  
  console.log("----------------------------------------");
  console.log("CURRENT SEED: " + activeSeed);
  console.log("If you love this look, copy this number!");
  console.log("Click the canvas to generate a new look.");
  console.log("----------------------------------------");
  
  noLoop(); 
}

function draw() {
  // Apply the active seed to lock p5's random and noise loops permanently for this frame
  randomSeed(activeSeed);
  noiseSeed(activeSeed);

  background(28, 30, 33); // Dark slate chalkboard

  let rx = 120;
  let ry = 100;
  let rw = 360;
  let rh = 280;
  let pad = OVERSHOOT;

  // Draw the frame - exactly ONE stroke pass per side 
  drawSingleFlatChalkStroke(rx - pad, ry, rx + rw + pad, ry);
  drawSingleFlatChalkStroke(rx + rw, ry - pad, rx + rw, ry + rh + pad);
  drawSingleFlatChalkStroke(rx + rw + pad, ry + rh, rx - pad, ry + rh);
  drawSingleFlatChalkStroke(rx, ry + rh + pad, rx, ry - pad);
}

/**
 * Click to cycle and discover fresh variations
 */
function mousePressed() {
  activeSeed = floor(random(1000000)); // Roll a fresh profile
  setup(); 
  redraw(); // Force canvas update
}

/**
 * Draws a single chalk stroke with unified, smooth noise landscapes
 */
function drawSingleFlatChalkStroke(x1, y1, x2, y2) {
  let distance = dist(x1, y1, x2, y2);
  let steps = distance * 1.5; 

  let dx = x2 - x1;
  let dy = y2 - y1;
  let nx = -dy / distance;
  let ny = dx / distance;

  // Use fixed offsets linked to the global seed loop
  let strokeOffsetNoiseY = random(5000);
  let strokeOffsetNoiseX = random(5000);

  for (let i = 0; i <= steps; i++) {
    let t = i / steps;

    let basePx = lerp(x1, x2, t);
    let basePy = lerp(y1, y2, t);

    // Smooth hand pressure shifting
    let pressureNoise = noise(strokeOffsetNoiseY + t * PRESSURE_WAVE);
    if (pressureNoise < 0.15) continue; 

    // Smooth width variation calculation
    let currentStrokeWidth = map(pressureNoise, 0.15, 1, MIN_STROKE_WIDTH, MAX_STROKE_WIDTH);

    // Subtle global hand wobble
    let wobble = (noise(strokeOffsetNoiseX + t * 3) - 0.5) * 2.5;

    let fiberCount = 6; 
    for (let f = 0; f < fiberCount; f++) {
      let fiberT = f / (fiberCount - 1); 
      
      let grainNoise = noise(
        strokeOffsetNoiseX + t * TEXTURE_GRIT, 
        strokeOffsetNoiseY + fiberT * 12.0
      );

      let displacement = map(grainNoise, 0, 1, -currentStrokeWidth / 2, currentStrokeWidth / 2);

      let finalX = basePx + nx * (wobble + displacement);
      let finalY = basePy + ny * (wobble + displacement);

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