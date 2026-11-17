"use client";

// Resources
import type { ComponentProps, JSX } from "react";
import { Progress as ProgressPrimitive } from "radix-ui";
import { cn } from "@/lib/utils";

/** The shadcn progress primitive with a theme-colored indicator. */
const Progress = ({
  className,
  value = 0,
  ...props
}: ComponentProps<typeof ProgressPrimitive.Root>): JSX.Element => (
  <ProgressPrimitive.Root
    data-slot={"progress"}
    value={value}
    className={cn("relative h-2 w-full overflow-hidden rounded-full bg-primary/20", className)}
    {...props}
  >
    <ProgressPrimitive.Indicator
      data-slot={"progress-indicator"}
      className={"h-full w-full flex-1 bg-primary transition-all"}
      style={{ transform: `translateX(-${100 - (value ?? 0)}%)` }}
    />
  </ProgressPrimitive.Root>
);

export { Progress };
