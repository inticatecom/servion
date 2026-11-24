// Resources
import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

/**
 * Combine conditional classes and resolve conflicting Tailwind utilities.
 *
 * @param inputs The class names to combine.
 *
 * @returns The resolved class names.
 */
export const cn = (...inputs: ClassValue[]): string => twMerge(clsx(inputs));
