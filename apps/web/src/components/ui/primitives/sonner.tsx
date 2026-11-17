"use client";

import type { CSSProperties, JSX } from "react";

import {
  CircleCheckIcon,
  InfoIcon,
  Loader2Icon,
  OctagonXIcon,
  TriangleAlertIcon,
} from "lucide-react";
import { Toaster as Sonner, type ToasterProps } from "sonner";

/** Inticate colors for the Sonner notification surface. */
const toastStyle: CSSProperties & Record<`--${string}`, string> = {
  "--border-radius": "var(--radius)",
  "--normal-bg": "var(--popover)",
  "--normal-border": "var(--border)",
  "--normal-text": "var(--popover-foreground)",
};

/** The shared shadcn notification host. */
const Toaster = ({ ...props }: ToasterProps): JSX.Element => (
  <Sonner
    theme={"dark"}
    className={"toaster group"}
    icons={{
      error: <OctagonXIcon className={"size-4"} />,
      info: <InfoIcon className={"size-4"} />,
      loading: <Loader2Icon className={"size-4 animate-spin"} />,
      success: <CircleCheckIcon className={"size-4"} />,
      warning: <TriangleAlertIcon className={"size-4"} />,
    }}
    style={toastStyle}
    {...props}
  />
);

export { Toaster };
