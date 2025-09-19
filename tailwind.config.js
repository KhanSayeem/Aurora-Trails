/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: ["class"],
  content: [
    "./Views/**/*.cshtml",
    "./Areas/**/*.cshtml",
    "./Pages/**/*.cshtml",
    "./Components/**/*.cshtml",
    "./wwwroot/js/**/*.js",
    "./Models/ViewModels/**/*.cs"
  ],
  theme: {
    container: {
      center: true,
      padding: "1.5rem",
      screens: {
        "2xl": "1280px"
      }
    },
    extend: {
      colors: {
        background: "hsl(222.2 47.4% 11.2%)",
        foreground: "hsl(210 40% 98%)",
        muted: {
          DEFAULT: "hsl(215 20.2% 65.1%)",
          foreground: "hsl(217.9 10.6% 64.9%)"
        },
        border: "hsl(214.3 31.8% 91.4%)",
        input: "hsl(214.3 31.8% 91.4%)",
        ring: "hsl(221.2 83.2% 53.3%)",
        primary: {
          DEFAULT: "hsl(221.2 83.2% 53.3%)",
          foreground: "hsl(210 40% 98%)"
        },
        secondary: {
          DEFAULT: "hsl(210 40% 96.1%)",
          foreground: "hsl(222.2 47.4% 11.2%)"
        },
        accent: {
          DEFAULT: "hsl(160.6 81.9% 39.6%)",
          foreground: "hsl(210 40% 98%)"
        },
        destructive: {
          DEFAULT: "hsl(0 84.2% 60.2%)",
          foreground: "hsl(210 40% 98%)"
        },
        card: {
          DEFAULT: "#ffffff",
          foreground: "hsl(222.2 47.4% 11.2%)"
        }
      },
      fontFamily: {
        sans: ["Inter", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "sans-serif"],
        display: ["Cal Sans", "Inter", "system-ui", "sans-serif"]
      },
      borderRadius: {
        lg: "0.75rem",
        xl: "1rem",
        '2xl': "1.5rem"
      },
      keyframes: {
        "accordion-down": {
          from: { height: "0" },
          to: { height: "var(--radix-accordion-content-height)" }
        },
        "accordion-up": {
          from: { height: "var(--radix-accordion-content-height)" },
          to: { height: "0" }
        }
      },
      animation: {
        "accordion-down": "accordion-down 0.2s ease-out",
        "accordion-up": "accordion-up 0.2s ease-out"
      },
      boxShadow: {
        card: "0 20px 45px -24px rgba(15, 23, 42, 0.45)",
        subtle: "0 12px 24px -20px rgba(15, 23, 42, 0.4)"
      }
    }
  },
  plugins: [require("@tailwindcss/forms"), require("tailwindcss-animate")]
}