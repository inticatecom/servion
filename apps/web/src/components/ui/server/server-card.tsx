// Resources
import { type JSX, useMemo } from "react";

// Components
import { Progress } from "@/components/ui/primitives/progress";
import ServerStatus from "@/components/ui/server/server-status";
import { MoreVertical } from "lucide-react";
import NavigationMenu from "@/components/ui/navigation/navigation-menu";

/** A game server displayed in the ownership dashboard. */
export interface GameServer {
  id: string;
  name: string;
  game: string;
  edition: string;
  region: string;
  address: string;
  status: "Online" | "Offline";
  players: number;
  slots: number;
  memory: number;
  capacity: number;
  cpu: number;
}

/** The properties of a server card. */
interface ServerCardProps {
  server: GameServer;
  onInspect: (server: GameServer) => void;
  onCopy: (server: GameServer) => void;
}

/** A compact server row with connection details and contextual actions. */
const ServerCard = ({ server, onInspect, onCopy }: ServerCardProps): JSX.Element => {
  const items = useMemo(
    () => [
      {
        label: "Server details",
        onSelect: (): void => {
          onInspect(server);
        }
      },
      {
        label: "Copy address",
        onSelect: (): void => {
          onCopy(server);
        }
      }
    ],
    [server, onInspect, onCopy]
  );

  return (
    <article
      className={
        "flex flex-wrap items-center gap-x-6 gap-y-3 border-b border-neutral-800 py-4 last:border-b-0 sm:flex-nowrap"
      }
    >
      <div className={"min-w-0 flex-1 basis-1/2 sm:basis-auto"}>
        <button
          type={"button"}
          onClick={() => {
            onInspect(server);
          }}
          className={
            "max-w-full truncate text-left text-sm font-medium text-white hover:underline focus-visible:outline-2 focus-visible:outline-white"
          }
        >
          {server.name}
        </button>
        <p className={"mt-1 text-xs text-neutral-500"}>
          {server.game}
          {" · "}
          {server.region}
        </p>
      </div>
      <div className={"w-20"}>
        <ServerStatus status={server.status} />
      </div>
      <p className={"w-24 shrink-0 whitespace-nowrap text-xs text-neutral-400"}>
        {server.players}
        {" / "}
        {server.slots}
        {" players"}
      </p>
      <div className={"w-28"}>
        <p className={"mb-2 text-xs text-neutral-500"}>
          {server.memory.toFixed(1)}
          {" / "}
          {server.capacity}
          {" GB"}
        </p>
        <Progress
          aria-label={`${server.name} memory usage`}
          value={server.capacity > 0 ? (server.memory / server.capacity) * 100 : 0}
          className={"h-1 rounded-sm bg-neutral-800"}
        />
      </div>
      <NavigationMenu label={`Actions for ${server.name}`} items={items}>
        <MoreVertical size={18} />
      </NavigationMenu>
    </article>
  );
};

export default ServerCard;
