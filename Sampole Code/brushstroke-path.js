// --- BRUSH CONTROLS ---
const MAX_STROKE_WIDTH = 30.5; 
const MIN_STROKE_WIDTH = 15.2; 
const TEXTURE_GRIT = 140;     
const PRESSURE_WAVE = 3.5;    

// --- HUMAN SKETCH CONTROLS ---
const PASSES = 3;             // How many times the hand retraces the body profile
const DRIFT_AMOUNT = 5.0;     // Tracking error (how far strokes misalign between passes)

let activeSeed = null; 
let humanPath = [];

function setup() {
  createCanvas(600, 600);
  
  if (activeSeed === null) {
    activeSeed = floor(random(1000000));
  }
  
  // Define the human silhouette path data
  // Types: 'L' for straight line to target, 'B' for Bezier curve (control1, control2, target)
  defineHumanSilhouette();
  
  console.log("--------------------------------------------------");
  console.log("🧍 CHALK SILHOUETTE CANVAS LOADED");
  console.log("CURRENT SEED: " + activeSeed);
  console.log("👉 CLICK the canvas to roll a new variation.");
  console.log("👉 PRESS 's' to save a PNG image!");
  console.log("--------------------------------------------------");
  
  noLoop(); 
}

function draw() {
     // noprotect  <-- THIS TELLS THE EDITOR TO IGNORE THE 500MS LIMIT!

  randomSeed(activeSeed);
  noiseSeed(activeSeed);

  background(26, 28, 30); // Dark slate chalkboard

  // Center and scale the silhouette to fit comfortably on screen
  push();
  translate(width / 2, height / 2 - 20);
  scale(1.1);

  // 1. Draw overlapping sketch passes to look authentically human-drawn
  for (let p = 0; p < PASSES; p++) {
    // Each sketch pass drifts slightly off-target
    let passOffsetX = random(-DRIFT_AMOUNT / 2, DRIFT_AMOUNT / 2);
    let passOffsetY = random(-DRIFT_AMOUNT / 2, DRIFT_AMOUNT / 2);
    
    // Store the starting point of the shape
    let startPt = humanPath[0];
    let currentX = startPt.x + passOffsetX;
    let currentY = startPt.y + passOffsetY;

    for (let i = 1; i < humanPath.length; i++) {
      let segment = humanPath[i];

      if (segment.type === 'L') {
        // Target coordinates
        let targetX = segment.x + passOffsetX;
        let targetY = segment.y + passOffsetY;

        // Render straight line segment
        drawGrainyChalkSegment(currentX, currentY, targetX, targetY);

        // Update tracking pen position
        currentX = targetX;
        currentY = targetY;
      } 
      else if (segment.type === 'B') {
        // Apply drift uniformly to all Bezier points in this pass
        let cx1 = segment.cx1 + passOffsetX;
        let cy1 = segment.cy1 + passOffsetY;
        let cx2 = segment.cx2 + passOffsetX;
        let cy2 = segment.cy2 + passOffsetY;
        let tx  = segment.x + passOffsetX;
        let ty  = segment.y + passOffsetY;

        // Render curved segment
        drawCurvedBezierChalkSegment(currentX, currentY, cx1, cy1, cx2, cy2, tx, ty);

        currentX = tx;
        currentY = ty;
      }
    }
  }
  pop();
}

function mousePressed() {
  activeSeed = floor(random(1000000));
  setup(); 
  redraw();
}

function keyPressed() {
  if (key === 's' || key === 'S') {
    saveCanvas("chalk_silhouette_" + activeSeed, "png");
  }
}

/**
 * Standard Straight Line Slice Renderer
 */
function drawGrainyChalkSegment(x1, y1, x2, y2) {
  // noprotect  <-- THIS TELLS THE EDITOR TO IGNORE THE 500MS LIMIT!
  let distance = dist(x1, y1, x2, y2);
  let steps = distance * 1.2; 

  let dx = x2 - x1;
  let dy = y2 - y1;
  let nx = -dy / distance;
  let ny = dx / distance;

  renderChalkPointsAlongVectors(steps, x1, y1, x2, y2, nx, ny, false, 0,0,0,0,0,0);
}

/**
 * Sub-samples a Bezier curve into tiny linear slices, computing normal vectors on the fly
 */

/**
 * Shatters a single long Bezier curve into multiple independent, 
 * overlapping sketchy strokes so it looks hand-sketched rather than a single machine stroke.
 */
function drawCurvedBezierChalkSegment(x1, y1, cx1, cy1, cx2, cy2, x2, y2) {
  // noprotect  <-- THIS TELLS THE EDITOR TO IGNORE THE 500MS LIMIT!
  // Approximate the total length of the curve
  let estimatedLength = dist(x1, y1, cx1, cy1) + dist(cx1, cy1, cx2, cy2) + dist(cx2, cy2, x2, y2);
  
  // 1. DETERMINE SKETCHY CHUNKS
  // Instead of 0.0 to 1.0 all at once, we break it into roughly 3 to 4 shorter micro-strokes
  let totalSubStrokes = random(3, 4); 
  let stepSize = 1.0 / totalSubStrokes;

  for (let s = 0; s < totalSubStrokes; s++) {
    // Define the base timeline window for this sub-stroke
    let tStart = s * stepSize;
    let tEnd = (s + 1) * stepSize;

    // HUMAN SKETCH ADJUSTMENT: 
    // Add an overlap factor so the strokes overshoot and cross each other's paths
    let overlap = 0.08; 
    if (s > 0) tStart -= random(0, overlap);
    if (s < totalSubStrokes - 1) tEnd += random(0, overlap);

    // Constrain to keep it within safe Bezier limits (0.0 to 1.0)
    tStart = constrain(tStart, 0, 1);
    tEnd = constrain(tEnd, 0, 1);

    // 2. RENDER THE SHATTERED SUB-STROKE
    // Calculate resolution just for this chunk
    let chunkLength = estimatedLength * (tEnd - tStart);
    let steps = floor(chunkLength * 0.4);
    if (steps < 2) steps = 2;

    let noiseSeedY = random(10000);
    let noiseSeedX = random(10000);

    // Each sub-stroke gets its own localized hand-slip drift
    let strokeDriftX = random(-2, 2);
    let strokeDriftY = random(-2, 2);

    for (let i = 0; i < steps; i++) {
      let subTCurrent = i / steps;
      let subTNext = (i + 1) / steps;

      // Map the local step back to the global Bezier timeline (0.0 to 1.0)
      let tCurrent = lerp(tStart, tEnd, subTCurrent);
      let tNext = lerp(tStart, tEnd, subTNext);

      // Extract the precise base coordinates on the curve
      let bx1 = bezierPoint(x1, cx1, cx2, x2, tCurrent) + strokeDriftX;
      let by1 = bezierPoint(y1, cy1, cy2, y2, tCurrent) + strokeDriftY;
      let bx2 = bezierPoint(x1, cx1, cx2, x2, tNext) + strokeDriftX;
      let by2 = bezierPoint(y1, cy1, cy2, y2, tNext) + strokeDriftY;

      let segmentDist = dist(bx1, by1, bx2, by2);
      if (segmentDist === 0) continue;

      // Perpendicular normal calculation
      let bdx = bx2 - bx1;
      let bdy = by2 - by1;
      let bnx = -bdy / segmentDist;
      let bny = bdx / segmentDist;

      // Pass down to the fiber rendering core
      renderChalkPointsAlongVectors(1, bx1, by1, bx2, by2, bnx, bny, true, tCurrent, noiseSeedX, noiseSeedY);
    }
  }
}

function drawCurvedBezierChalkSegment2(x1, y1, cx1, cy1, cx2, cy2, x2, y2) {
  // Approximate length of a bezier curve to gauge step resolution
  let estimatedLength = dist(x1, y1, cx1, cy1) + dist(cx1, cy1, cx2, cy2) + dist(cx2, cy2, x2, y2);
  let steps = floor(estimatedLength * 1.2);

  let noiseSeedY = random(10000);
  let noiseSeedX = random(10000);

  // We step through the Bezier timeline (t2 from 0.0 to 1.0)
  for (let i = 0; i < steps; i++) {
    let tCurrent = i / steps;
    let tNext = (i + 1) / steps;

    // Find current base coordinate on the curve
    let bx1 = bezierPoint(x1, cx1, cx2, x2, tCurrent);
    let by1 = bezierPoint(y1, cy1, cy2, y2, tCurrent);

    // Find next immediate coordinate to determine the instantaneous direction vector
    let bx2 = bezierPoint(x1, cx1, cx2, x2, tNext);
    let by2 = bezierPoint(y1, cy1, cy2, y2, tNext);

    let segmentDist = dist(bx1, by1, bx2, by2);
    if (segmentDist === 0) continue;

    // Calculate normal perpendicular vector for this micro-slice
    let bdx = bx2 - bx1;
    let bdy = by2 - by1;
    let bnx = -bdy / segmentDist;
    let bny = bdx / segmentDist;

    // Render this micro-slice using a specialized single-step execution
    renderChalkPointsAlongVectors(1, bx1, by1, bx2, by2, bnx, bny, true, tCurrent, noiseSeedX, noiseSeedY, x1, y1, x2);
  }
}

/**
 * Core Core Core Engine: Distributes chalk particles along path segments
 */
function renderChalkPointsAlongVectors(steps, x1, y1, x2, y2, nx, ny, isBezier, bezT, nsX, nsY) {
  // noprotect  <-- THIS TELLS THE EDITOR TO IGNORE THE 500MS LIMIT!
  let noiseSeedY = isBezier ? nsY : random(10000);
  let noiseSeedX = isBezier ? nsX : random(10000);

  for (let i = 0; i <= steps; i++) {
    // Interpolation factor along this specific segment
    let segmentT = steps === 0 ? 0 : i / steps;
    
    // Global path progress tracker used for noise continuity
    let globalT = isBezier ? bezT : segmentT;

    let basePx = lerp(x1, x2, segmentT);
    let basePy = lerp(y1, y2, segmentT);

    let pressureNoise = noise(noiseSeedY + globalT * PRESSURE_WAVE);
    if (pressureNoise < 0.15) continue; 

    let currentStrokeWidth = map(pressureNoise, 0.15, 1, MIN_STROKE_WIDTH, MAX_STROKE_WIDTH);
    let wobble = (noise(noiseSeedX + globalT * 3) - 0.5) * 1.5;

    let fiberCount = 4; 
    for (let f = 0; f < fiberCount; f++) {
      let fiberT = f / (fiberCount - 1); 
      
      let grainNoise = noise(
        noiseSeedX + globalT * TEXTURE_GRIT, 
        noiseSeedY + fiberT * 12.0
      );

      let displacement = map(grainNoise, 0, 1, -currentStrokeWidth / 2, currentStrokeWidth / 2);

      let finalX = basePx + nx * (wobble + displacement);
      let finalY = basePy + ny * (wobble + displacement);

      finalX += random(-0.3, 0.3);
      finalY += random(-0.3, 0.3);

      let currentAlpha = map(pressureNoise, 0.15, 1, 40, 190);
      let currentWeight = map(pressureNoise, 0.15, 1, 0.5, 1.8);

      stroke(245, 245, 243, currentAlpha * random(0.6, 1.3));
      strokeWeight(currentWeight * random(0.7, 1.3));

      point(finalX, finalY);
    }
  }
}

/**
 * Hardcoded relative path arrays mapping out a stylized artist mannequin figure centered at (0,0)
 */
function defineHumanSilhouette() {
  humanPath = [
    { type: 'M', x: 0, y: -160 }, // Move to top of Head
    
    // Head profile
    { type: 'B', cx1: 30, cy1: -160, cx2: 30, cy2: -100, x: 0, y: -100 }, // Right Side Head
    { type: 'B', cx1: -30, cy1: -100, cx2: -30, cy2: -160, x: 0, y: -160 }, // Left Side Head
    
    // Re-anchor to neck base to continue full outline
    { type: 'M', x: 0, y: -100 },
    { type: 'L', x: 15, y: -85 },   // Right Neck
    { type: 'L', x: 70, y: -65 },   // Right Shoulder
    
    // Right Arm (Bicep & Forearm)
    { type: 'B', cx1: 100, cy1: -20, cx2: 120, cy2: 40, x: 90, y: 100 }, 
    { type: 'L', x: 70, y: 95 },    // Hand/Wrist edge
    { type: 'B', cx1: 85, cy1: 40, cx2: 75, cy2: -10, x: 45, y: -35 }, // Inner arm pit
    
    // Torso / Right Hip
    { type: 'L', x: 35, y: 30 },    // Waist cut
    { type: 'B', cx1: 50, cy1: 60, cx2: 55, cy2: 90, x: 40, y: 120 }, // Hip flare
    
    // Right Leg
    { type: 'L', x: 55, y: 220 },   // Out knee
    { type: 'L', x: 45, y: 310 },   // Out ankle
    { type: 'L', x: 15, y: 310 },   // Foot base
    { type: 'B', cx1: 25, cy1: 220, cx2: 30, cy2: 150, x: 5, y: 125 }, // Crotch inner thigh
    
    // Left Leg
    { type: 'B', cx1: -30, cy1: 150, cx2: -25, cy2: 220, x: -15, y: 310 }, // Left inner thigh
    { type: 'L', x: -45, y: 310 },  // Left foot base
    { type: 'L', x: -55, y: 220 },  // Left out knee
    { type: 'B', cx1: -55, cy1: 90, cx2: -50, cy2: 60, x: -40, y: 120 }, // Left hip flare
    
    // Torso / Left Hip
    { type: 'L', x: -35, y: 30 },   // Waist cut
    { type: 'B', cx1: -75, cy1: -10, cx2: -85, cy2: 40, x: -70, y: 95 }, // Left inner arm pit
    
    // Left Arm
    { type: 'L', x: -90, y: 100 },  // Hand edge
    { type: 'B', cx1: -120, cy1: 40, cx2: -100, cy2: -20, x: -70, y: -65 }, // Left Outer Arm
    
    // Close back to Neck
    { type: 'L', x: -15, y: -85 },  // Left Shoulder up
    { type: 'L', x: 0, y: -100 }    // Back to center neck base
  ];
}