// Resources
import type { JSX } from "react";
import type { GameServer } from "@/components/ui/server/server-card";

// Components
import ServerStatus from "@/components/ui/server/server-status";
import { Dialog, DialogContent, DialogTitle, DialogDescription } from "@/components/ui/primitives/dialog";
import { Button } from "@/components/ui/primitives/button";

/** The properties of the server details dialog. */
interface ServerDetailsProps {
  server: GameServer;
  handleClose: () => void;
  onCopy: (server: GameServer) => void;
}

/** Show connection details without leaving the server collection. */
const ServerDetails = ({ server, handleClose, onCopy }: ServerDetailsProps): JSX.Element => (
  <Dialog
    open
    onOpenChange={open => {
      if (!open) {
        handleClose();
      }
    }}
  >
    <DialogContent className={"sm:max-w-md rounded-lg border-neutral-700 bg-[#181818] text-neutral-200"}>
      <DialogTitle className={"pr-8 text-xl font-semibold tracking-tight text-white"}>{server.name}</DialogTitle>
      <DialogDescription>
        {server.game}
        {" · "}
        {server.region}
      </DialogDescription>
      <dl className={"my-6 space-y-4 text-sm"}>
        <div className={"flex justify-between"}>
          <dt className={"text-neutral-500"}>{"Status"}</dt>
          <dd>
            <ServerStatus status={server.status} />
          </dd>
        </div>
        <div className={"flex justify-between"}>
          <dt className={"text-neutral-500"}>{"Server ID"}</dt>
          <dd className={"font-mono"}>{server.id}</dd>
        </div>
        <div>
          <dt className={"mb-2 text-neutral-500"}>{"Connection address"}</dt>
          <dd className={"break-all rounded-lg border border-neutral-800 bg-[#121212] p-3 font-mono text-xs"}>
            {server.address}
          </dd>
        </div>
      </dl>
      <Button
        onClick={() => {
          onCopy(server);
        }}
        className={"w-full"}
      >
        {"Copy address"}
      </Button>
      <p className={"mt-4 text-xs leading-5 text-neutral-500"}>
        {"Preview data. Server controls will be available when your account is connected."}
      </p>
    </DialogContent>
  </Dialog>
);

export default ServerDetails;
