// Resources
import type { ClassName, Children } from "@servion/utility";
import { cn } from "@/utility/general";

/** A basic card to containerize elements with. */
const Card = ({ className, children }: ClassName & Children) => (
  <div className={cn("p-10 rounded-lg bg-primary border border-white/10", className)}>
    {children}
  </div>
);

export default Card;
