// Resources
import { defineConfig } from "vite";
import appConfiguration, { environment } from "@servion/config";

// Plugins
import { nitro } from "nitro/vite";
import tailwindcss from "@tailwindcss/vite";
import { tanstackStart } from "@tanstack/react-start/plugin/vite";
import viteReact from "@vitejs/plugin-react";
import path from "node:path";

/** The Vite configuration. */
const configuration = defineConfig({
  envDir: path.resolve(import.meta.dirname, "../.."),
  envPrefix: ["PUBLIC_"],
  plugins: [
    tailwindcss(),
    tanstackStart({
      srcDirectory: "src",
    }),
    viteReact(),
    nitro({
      routeRules: {
        "/api/**": { proxy: `http://localhost:${environment.SERVER_PORT}/api/**` },
      },
    }),
  ],
  resolve: {
    tsconfigPaths: true,
  },
  server: {
    port: appConfiguration.webPort,
  },
});

export default configuration;
