// Resources
import type { JSX } from "react";

// Components
import { Badge } from "@/components/ui/primitives/badge";

/** The properties of a server state badge. */
interface ServerStatusProps {
  status: "Online" | "Offline";
}

/** A compact status badge using the dashboard palette. */
const ServerStatus = ({ status }: ServerStatusProps): JSX.Element => (
  <Badge
    variant={"outline"}
    className={
      status === "Online"
        ? "rounded border-emerald-500/20 bg-emerald-500/10 text-emerald-400"
        : "rounded border-neutral-500/20 bg-neutral-500/10 text-neutral-400"
    }
  >
    {status}
  </Badge>
);

export default ServerStatus;
