export function initialize(element, instance) {
    // Select DOM elements
    const overlay = element.querySelector(".bottom-sheet-overlay");
    const content = element.querySelector(".bottom-sheet-content");
    const icon = element.querySelector(".bottom-sheet-drag-icon");

    // Global variables for tracking drag events
    const controller = new AbortController();
    let isDragging = false, startY, startHeight;

    const setHeight = (height) => {
        content.style.height = `${height}vh`;
        element.classList.toggle("fullscreen", height === 100);
    }

    const show = () => {
        element.classList.add("show");
        document.body.style.overflowY = "hidden";
        setHeight(50);
    }

    const hide = () => {
        element.classList.remove("show");
        document.body.style.overflowY = '';
        instance.invokeMethodAsync("NotifyCloseRequested");
    }

    // Sets initial drag position, content height and add dragging class to the bottom sheet
    const startdrag = (e) => {
        isDragging = true;
        startY = e.pageY || e.touches?.[0].pageY;
        startHeight = Number.parseInt(content.style.height);
        element.classList.add("dragging");
    }

    // Calculates the new height for the sheet content and call the updateSheetHeight function
    const dragging = (e) => {
        if (!isDragging) return;
        const delta = startY - (e.pageY || e.touches?.[0].pageY);
        const newHeight = startHeight + delta / window.innerHeight * 100;
        setHeight(newHeight);
    }

    // Determines whether to hide, set to fullscreen, or set to default 
    // height based on the current height of the sheet content
    const enddrag = () => {
        isDragging = false;
        element.classList.remove("dragging");
        const height = Number.parseInt(content.style.height);
        height < 25 ? hide() : height > 75 ? setHeight(100) : setHeight(50);
    }

    icon.addEventListener("mousedown", startdrag, { signal: controller.signal });
    icon.addEventListener("touchstart", startdrag, { signal: controller.signal });

    document.addEventListener("mousemove", dragging, { signal: controller.signal });
    document.addEventListener("touchmove", dragging, { signal: controller.signal });

    document.addEventListener("mouseup", enddrag, { signal: controller.signal });
    document.addEventListener("touchend", enddrag, { signal: controller.signal });

    overlay.addEventListener("click", hide);

    return {
        show,
        hide,
        dispose: () => {
            controller.abort();
        }
    }
}