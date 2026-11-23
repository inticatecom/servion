// Resources
import type { Children } from "@servion/utility";

/** The properties of a callout component. */
export interface CalloutProperties extends Children<string> {
  title?: string;
  type: CalloutType;
}

/** The level or type of callout. */
export type CalloutType = "success" | "warning" | "error";

/** A base callout component for displaying states of operations. */
const Callout = ({ children, type, title }: CalloutProperties) => (
  <div
    className={"prose bg-red-500/10 border border-red-500/20 p-3 rounded-lg"}>
    <h4>{title ?? `${type.charAt(0).toUpperCase()}${type.slice(1)}`}</h4>
    <p>{children}</p>
  </div>
);

export default Callout;
