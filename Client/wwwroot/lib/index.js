const key = "lumexui.theme";
const theme = localStorage.getItem(key);
const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;

const isDark =
    theme === "dark" ||
    (theme === "system" || theme === null) && prefersDark;

document.documentElement.classList.toggle("dark", isDark);