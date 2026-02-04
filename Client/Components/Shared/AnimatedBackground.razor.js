export function initialize(elementId) {
    particlesJS(elementId, {
        particles: {
            number: { value: 50, density: { enable: true, value_area: 1000 } },
            color: { value: "#2B7FFF" },
            shape: { type: "circle" },
            opacity: { value: 0.5, random: true },
            size: { value: 4, random: true },
            move: {
                enable: true,
                speed: 0.4,
                random: true,
                out_mode: "out"
            }
        },
        interactivity: {
            detect_on: "window",
            events: {
                onhover: { enable: true, mode: "repulse" }
            },
            modes: {
                repulse: { distance: 80, duration: 0.3 }
            }
        },
        retina_detect: true
    });
}