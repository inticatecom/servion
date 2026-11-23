// Resources
import type { Children } from "@servion/utility";

// Components

// Icons
import { Ban } from "lucide-react";
import { FeaturedIcon } from "../foundations/featured-icon/featured-icon";

/** The properties of a callout component. */
export interface CalloutProperties extends Children<string> {
  title?: string;
  type: CalloutType;
}

/** The level or type of callout. */
export type CalloutType = "success" | "warning" | "error";

/** A base callout component for displaying states of operations. */
const Callout = ({ children, type, title }: CalloutProperties) => (
  <div className={"prose bg-red-500/10 border border-red-500/20 p-3 rounded-lg"}>
    <h4 className={"my-0 flex gap-3 items-center"}>
      <FeaturedIcon color={"error"} icon={Ban} theme={"outline"} />
      {title ?? `${type.charAt(0).toUpperCase()}${type.slice(1)}`}
    </h4>
    <p>{children}</p>
  </div>
);

export default Callout;
