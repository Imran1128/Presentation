const canvas = new fabric.Canvas('canvas', {
    width: 1300,
    height: 600,
});

const modes = {
    text: 'text',
    drawing: 'drawing',
    erase: 'erase',
    rectangle: 'rectangle',
    circle: 'circle',
    arrow: 'arrow',
    image: 'image',
};

let currentMode = null;
let isDrawing = false;

// Reset event listeners on the canvas
function resetCanvasEvents() {
    canvas.off('mouse:down');
    canvas.off('mouse:move');
    canvas.off('mouse:up');
    isDrawing = false;
}

// Handle switching between tools
function setTool(tool) {
    resetCanvasEvents();
    currentMode = tool;

    if (tool === 'draw') {
        enableDrawingMode();
    } else if (tool === 'erase') {
        enableEraseMode();
    } else if (tool === 'rectangle') {
        enableRectangleMode();
    } else if (tool === 'circle') {
        enableCircleMode();
    } else if (tool === 'arrow') {
        enableArrowMode();
    } else if (tool === 'image') {
        enableImageMode();
    }
}

// Enable text tool
function toggleMode(mode) {
    resetCanvasEvents();
    currentMode = mode;

    if (mode === modes.text) {
        canvas.on('mouse:down', function (event) {
            const pointer = canvas.getPointer(event.e);
            
            if (!userInput) return; 

            const textbox = new fabric.Text("dfg", {
                left: pointer.x,
                top: pointer.y,
                fontFamily: document.getElementById('fontSelect').value,
                fontSize: parseInt(document.getElementById('fontSizeSelect').value),
                fill: document.getElementById('colorPicker').value,
            });

            canvas.add(textbox);
        });
    }
}


function enableDrawingMode() {
    canvas.isDrawingMode = true;
    canvas.freeDrawingBrush.color = document.getElementById('colorPicker').value;
    canvas.freeDrawingBrush.width = 2;
}


function enableEraseMode() {
    canvas.isDrawingMode = false;
    canvas.on('mouse:down', function (event) {
        const pointer = canvas.getPointer(event.e);
        const objects = canvas.getObjects();
        objects.forEach((obj) => {
            if (obj.containsPoint(pointer)) {
                canvas.remove(obj);
            }
        });
    });
}


function enableRectangleMode() {
    let rect;
    let startX, startY;

    canvas.on('mouse:down', function (event) {
        const pointer = canvas.getPointer(event.e);
        startX = pointer.x;
        startY = pointer.y;

        rect = new fabric.Rect({
            left: startX,
            top: startY,
            fill: 'transparent',
            stroke: document.getElementById('colorPicker').value,
            strokeWidth: 2,
            width: 0,
            height: 0,
        });

        canvas.add(rect);
    });

    canvas.on('mouse:move', function (event) {
        if (!rect) return;

        const pointer = canvas.getPointer(event.e);
        rect.set({
            width: pointer.x - startX,
            height: pointer.y - startY,
        });

        canvas.renderAll();
    });

    canvas.on('mouse:up', function () {
        rect = null;
    });
}


function enableCircleMode() {
    let circle;
    let startX, startY;

    canvas.on('mouse:down', function (event) {
        const pointer = canvas.getPointer(event.e);
        startX = pointer.x;
        startY = pointer.y;

        circle = new fabric.Circle({
            left: startX,
            top: startY,
            radius: 0,
            fill: 'transparent',
            stroke: document.getElementById('colorPicker').value,
            strokeWidth: 2,
        });

        canvas.add(circle);
    });

    canvas.on('mouse:move', function (event) {
        if (!circle) return;

        const pointer = canvas.getPointer(event.e);
        const radius = Math.sqrt(
            Math.pow(pointer.x - startX, 2) + Math.pow(pointer.y - startY, 2)
        );
        circle.set({ radius });

        canvas.renderAll();
    });

    canvas.on('mouse:up', function () {
        circle = null;
    });
}

function enableImageMode() {
    const imgURL = prompt('Enter Image URL:');
    if (!imgURL) return;

    fabric.Image.fromURL(imgURL, function (img) {
        img.set({
            left: 100,
            top: 100,
            scaleX: 0.2,
            scaleY: 0.2,
        });
        canvas.add(img);
    });
}


function zoom(direction) {
    const zoom = canvas.getZoom();
    canvas.setZoom(direction === 'in' ? zoom * 1.1 : zoom * 0.9);
}


function setBackground() {
    const color = document.getElementById('colorPicker').value;
    canvas.setBackgroundColor(color, canvas.renderAll.bind(canvas));
}





async function exportToPDF() {
    const pdfDoc = await PDFLib.PDFDocument.create();
    const page = pdfDoc.addPage([canvas.width, canvas.height]);

    const img = await pdfDoc.embedPng(canvas.toDataURL('image/png'));
    page.drawImage(img, { x: 0, y: 0, width: canvas.width, height: canvas.height });

   
}



const mycanvas = document.getElementById('mycanvas'); // Declare canvas variable only once



document.getElementById('saveCanvas').addEventListener('click', async () => {
    try {
        const canvasData = canvas.toDataURL('Img/png');
        const response = await fetch('Home/PresentationPost', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ imageData: canvasData })
        });

        // Check if the response is successful
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        // Corrected: Use response.json() instead of response.jSON()
        const result = await response.json();
        alert(result.message);
    } catch (error) {
        console.error('Error:', error.message);
        alert('Something went wrong!');
    }
});

