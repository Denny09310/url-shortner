export function initialize(elementId) {
    particlesJS(elementId, {
        particles: {
            number: { value: 60 },
            size: { value: 2 },
            color: { value: "#6366f1" },
            opacity: { value: 0.3 },
            move: { speed: 0.6 }
        },
        interactivity: {
            events: {
                onhover: { enable: true, mode: "repulse" }
            }
        }
    });
}