// Resources
import { defineConfig } from "vite";
import appConfiguration, { environment } from "@servion/config";
import path from "node:path";

// Plugins
import { nitro } from "nitro/vite";
import tailwindcss from "@tailwindcss/vite";
import { tanstackStart } from "@tanstack/react-start/plugin/vite";
import viteReact from "@vitejs/plugin-react";
import { paraglideVitePlugin } from "@inlang/paraglide-js";

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
    paraglideVitePlugin({
      cookieName: "locale",
      emitTsDeclarations: true,
      outdir: "./src/paraglide",
      project: "./project.inlang",
      strategy: ["cookie", "preferredLanguage", "baseLocale"],
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
