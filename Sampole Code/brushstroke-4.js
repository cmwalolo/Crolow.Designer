const MAX_STROKE_WIDTH = 9.0; // Max width when pressing hard
const MIN_STROKE_WIDTH = 1.5; // Min width when lifting the chalk
const TEXTURE_GRIT = 140;     // HIGHER = tighter, crispier chalk grain clusters
const PRESSURE_WAVE = 4;      // LOWER = longer, smoother transitions in hand pressure
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

  let pad = OVERSHOOT;

  // Draw the frame - exactly ONE stroke pass per side with dynamic width shifting
  drawSingleFlatChalkStroke(rx - pad, ry, rx + rw + pad, ry);
  drawSingleFlatChalkStroke(rx + rw, ry - pad, rx + rw, ry + rh + pad);
  drawSingleFlatChalkStroke(rx + rw + pad, ry + rh, rx - pad, ry + rh);
  drawSingleFlatChalkStroke(rx, ry + rh + pad, rx, ry - pad);
}

/**
 * Draws a single chalk stroke where width, opacity, and particle density 
 * all vary dynamically based on Perlin noise hand-pressure simulation.
 */
function drawSingleFlatChalkStroke(x1, y1, x2, y2) {
  let distance = dist(x1, y1, x2, y2);
  let steps = distance * 1.5; 

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

    // 1. Hand Pressure Matrix
    let pressureNoise = noise(noiseSeedY + t * PRESSURE_WAVE);
    if (pressureNoise < 0.18) continue; // Chalk skipping threshold

    // 2. DYNAMIC WIDTH VARIATION: Map pressure directly to the lateral stroke width limit
    let currentStrokeWidth = map(pressureNoise, 0.18, 1, MIN_STROKE_WIDTH, MAX_STROKE_WIDTH);

    // 3. Structural Wobble (Slight hand tracking error)
    let wobble = (noise(noiseSeedX + t * 4) - 0.5) * 3.5;

    // 4. Flat-Side Expansion with dynamic boundaries
    let fiberCount = 6; 
    for (let f = 0; f < fiberCount; f++) {
      let fiberT = f / (fiberCount - 1); 
      
      let grainNoise = noise(
        noiseSeedX + t * TEXTURE_GRIT, 
        noiseSeedY + fiberT * 12.0
      );

      // Distribute particles across the newly calculated dynamic stroke width
      let displacement = map(grainNoise, 0, 1, -currentStrokeWidth / 2, currentStrokeWidth / 2);

      // Final calculation using the dynamic width boundary
      let finalX = basePx + nx * (wobble + displacement);
      let finalY = basePy + ny * (wobble + displacement);

      // Microscopic powder scattering
      finalX += random(-0.4, 0.4);
      finalY += random(-0.4, 0.4);

      // Opacity and weight also react to the localized pressure
      let currentAlpha = map(pressureNoise, 0.18, 1, 25, 200);
      let currentWeight = map(pressureNoise, 0.18, 1, 0.5, 2.4);

      stroke(245, 245, 243, currentAlpha * random(0.6, 1.3));
      strokeWeight(currentWeight * random(0.7, 1.3));

      point(finalX, finalY);
    }
  }
}