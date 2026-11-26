// Resources
import { useCallback } from "react";
import { m } from "@/paraglide/messages";

// Components
import { Button } from "./base/buttons/button";

/** A page representing the state of a missing page. */
const NotFound = () => {
  /** Brings the client to the previous page. */
  const handleBack = useCallback(() => {
    window.history.back();
  }, []);

  return (
    <div className={"w-full h-screen flex justify-center items-center"}>
      <div className={"prose text-center"}>
        <h1>{m["headings.not_found"]()}</h1>
        <p>{m["pages.not_found.description"]()}</p>
        <div className={"flex justify-center items-center gap-2"}>
          <Button color={"secondary"} onClick={handleBack}>
            {m["common.back"]()}
          </Button>
          <Button href={"/"}>{m["common.home"]()}</Button>
        </div>
      </div>
    </div>
  );
};

export default NotFound;
