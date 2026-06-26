//https://editor.p5js.org/
function setup() {
  createCanvas(600, 400);
  noLoop(); // Draw once since it's a static chalk sketch
}

function draw() {
  background(40); // Dark chalkboard background

  // Draw three chalk rectangles with different properties
  drawChalkRect(100, 100, 150, 200, 3, color(255));       // White chalk
  drawChalkRect(300, 80, 200, 120, 4, color(255, 150, 150)); // Pink chalk
  drawChalkRect(250, 240, 220, 100, 2, color(150, 220, 255)); // Blue chalk
}

/**
 * Draws a rectangle with a textured, noisy chalk border
 * @param {number} x - Top-left X coordinate
 * @param {number} y - Top-left Y coordinate
 * @param {number} w - Width of the rectangle
 * @param {number} h - Height of the rectangle
 * @param {number} density - How many overlapping lines to simulate chalk thickness
 * @param {color} chalkColor - The color of the chalk
 */
function drawChalkRect(x, y, w, h, density, chalkColor) {
  stroke(chalkColor);
  
  // Multiple passes create the dusty, layered chalk look
  for (let d = 0; d < density; d++) {
    // Set low opacity for individual strokes so they blend roughly
    let alpha = random(80, 150); 
    stroke(red(chalkColor), green(chalkColor), blue(chalkColor), alpha);
    strokeWeight(random(1, 2.5));
    
    // Define the 4 corners of this specific pass with slight variations
    let offset = 2;
    let x1 = x + random(-offset, offset);
    let y1 = y + random(-offset, offset);
    let x2 = x + w + random(-offset, offset);
    let y2 = y + h + random(-offset, offset);

    // Draw the 4 noisy edges
    drawNoisyLine(x1, y1, x2, y1); // Top
    drawNoisyLine(x2, y1, x2, y2); // Right
    drawNoisyLine(x2, y2, x1, y2); // Bottom
    drawNoisyLine(x1, y2, x1, y1); // Left
  }
}

/**
 * Breaks a line down into small segments and distorts them using Perlin noise
 */
function drawNoisyLine(xStart, yStart, xEnd, yEnd) {
  let distance = dist(xStart, yStart, xEnd, yEnd);
  let steps = distance; // One step per pixel for maximum texture detail
  
  // Seed the noise differently for every single line pass
  let noiseSeedX = random(1000);
  let noiseSeedY = random(1000);

  beginShape();
  for (let i = 0; i <= steps; i++) {
    let t = i / steps;
    
    // Linearly interpolate to find the perfect straight path coordinate
    let currentX = lerp(xStart, xEnd, t);
    let currentY = lerp(yStart, yEnd, t);

    // Use Perlin noise to calculate a rough displacement
    // Multiplying 't' controls the frequency (roughness) of the noise
    let noiseX = noise(noiseSeedX + t * 15) - 0.5; 
    let noiseY = noise(noiseSeedY + t * 15) - 0.5;

    // Chalk intensity: scale the displacement (amplitude)
    let magnitude = 3; 
    
    // Apply the noise displacement and add a tiny bit of random dust
    let finalX = currentX + noiseX * magnitude + random(-0.5, 0.5);
    let finalY = currentY + noiseY * magnitude + random(-0.5, 0.5);

    vertex(finalX, finalY);
  }
  endShape();
}