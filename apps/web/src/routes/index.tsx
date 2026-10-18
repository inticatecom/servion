// Resources
import { type JSX, useEffect, useState } from "react";
import { createFileRoute } from "@tanstack/react-router";

// Components
import { toast } from "sonner";
import { Input } from "@/components/ui/primitives/input";
import { Button } from "@/components/ui/primitives/button";
import NavigationBar, { type NavigationLink } from "@/components/ui/navigation/navigation-bar";
import { Search, Server } from "lucide-react";
import ServerDetails from "@/components/ui/server/server-details";
import ServerCard, { type GameServer } from "@/components/ui/server/server-card";
import { useGetApiServers } from "@/generated/api";

/** Placeholder destinations for the dashboard navigation. */
const navigationLinks: NavigationLink[] = [
  { href: "#servers", label: "Servers" },
  { href: "#billing", label: "Billing" },
  { href: "#support", label: "Support" },
  { href: "#settings", label: "Settings" },
];

/** Preview data until the authenticated server endpoint is connected. */
const servers: GameServer[] = [
  {
    address: "overworld.example.com:25565",
    capacity: 8,
    cpu: 24,
    edition: "Paper 1.21.4",
    game: "Minecraft",
    id: "SRV-001",
    memory: 3.2,
    name: "The Overworld",
    players: 12,
    region: "Vancouver, CA",
    slots: 30,
    status: "Online",
  },
  {
    address: "rust.example.com:28015",
    capacity: 16,
    cpu: 62,
    edition: "Vanilla",
    game: "Rust",
    id: "SRV-002",
    memory: 9.6,
    name: "No Man’s Land",
    players: 48,
    region: "Seattle, US",
    slots: 100,
    status: "Online",
  },
  {
    address: "cs2.example.com:27015",
    capacity: 4,
    cpu: 18,
    edition: "Competitive",
    game: "Counter-Strike 2",
    id: "SRV-003",
    memory: 2.1,
    name: "After Hours",
    players: 8,
    region: "Seattle, US",
    slots: 10,
    status: "Online",
  },
  {
    address: "valheim.example.com:2456",
    capacity: 4,
    cpu: 0,
    edition: "Crossplay",
    game: "Valheim",
    id: "SRV-004",
    memory: 0,
    name: "North of the Mist",
    players: 0,
    region: "Vancouver, CA",
    slots: 10,
    status: "Offline",
  },
];

/** Copy a connection address and announce the result. */
const handleCopy = async (server: GameServer): Promise<void> => {
  try {
    await navigator.clipboard.writeText(server.address);
    toast.success(`Copied ${server.name} address.`);
  } catch {
    toast.error("Unable to copy address.", { description: server.address });
  }
};

/** The server ownership dashboard mounted at the root. */
const Home = (): JSX.Element => {
  const [query, setQuery] = useState("");
  const [filter, setFilter] = useState("All servers");
  const [selected, setSelected] = useState<GameServer>();
  const visible = servers.filter(
    (server) =>
      (filter === "All servers" || server.status === filter) &&
      `${server.name} ${server.game} ${server.region}`.toLowerCase().includes(query.toLowerCase()),
  );
  const { data } = useGetApiServers();

  useEffect(() => {
    console.log(data);
  }, [data]);

  return (
    <main className={"mx-auto min-h-screen max-w-5xl px-5 py-6 sm:px-8 sm:py-8"}>
      <NavigationBar activeHref={"#servers"} links={navigationLinks} />

      <section id={"servers"} aria-labelledby={"servers-heading"} className={"mt-8"}>
        <div className={"mb-6 flex flex-wrap items-center justify-between gap-4"}>
          <div className={"flex items-center gap-3"}>
            <h2 id={"servers-heading"} className={"text-base font-medium text-white"}>
              {"Servers"}
            </h2>
            <span className={"text-xs text-neutral-500"}>{servers.length}</span>
          </div>
          <label className={"relative w-full sm:w-64"}>
            <span className={"sr-only"}>{"Search servers"}</span>
            <Search
              size={16}
              className={"pointer-events-none absolute left-3 top-3 text-neutral-500"}
            />
            <Input
              type={"search"}
              value={query}
              onChange={(event) => {
                setQuery(event.target.value);
              }}
              placeholder={"Find a server…"}
              className={
                "h-9 w-full rounded border border-neutral-800 bg-transparent pl-9 pr-3 text-sm text-white placeholder:text-neutral-600 focus:border-neutral-500 focus:ring-0"
              }
            />
          </label>
        </div>
        <div role={"group"} aria-label={"Filter by server status"} className={"mb-6 flex gap-1"}>
          {["All servers", "Online", "Offline"].map((status) => (
            <Button
              key={status}
              type={"button"}
              variant={"ghost"}
              size={"sm"}
              aria-pressed={filter === status}
              onClick={() => {
                setFilter(status);
              }}
              className={`rounded px-3 py-1.5 text-xs transition focus-visible:outline-2 focus-visible:outline-white ${filter === status ? "bg-neutral-800 text-white" : "text-neutral-500 hover:text-neutral-200"}`}
            >
              {status}
            </Button>
          ))}
        </div>
        <div className={"border-y border-neutral-800"}>
          {visible.map((server) => (
            <ServerCard
              key={server.id}
              server={server}
              onInspect={setSelected}
              onCopy={(item) => {
                void handleCopy(item);
              }}
            />
          ))}
        </div>
        {visible.length === 0 && (
          <div className={"px-6 py-12 text-center"}>
            <Server className={"mx-auto mb-4 text-neutral-600"} size={28} />
            <h3 className={"text-sm font-medium text-white"}>{"No servers found"}</h3>
            <p className={"mt-2 text-sm text-neutral-500"}>
              {"Try a different name, game, or status."}
            </p>
            <Button
              type={"button"}
              onClick={() => {
                setQuery("");
                setFilter("All servers");
              }}
              variant={"link"}
              className={"mt-5 text-sm"}
            >
              {"Clear filters"}
            </Button>
          </div>
        )}
      </section>

      {selected && (
        <ServerDetails
          server={selected}
          handleClose={() => {
            setSelected(undefined);
          }}
          onCopy={(server) => {
            void handleCopy(server);
          }}
        />
      )}
    </main>
  );
};

/** The route for the file. */
export const Route = createFileRoute("/")({ component: Home });
