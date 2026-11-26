// Components
import { useCallback } from "react";
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
        <h1>{"Page not Found"}</h1>
        <p>
          {
            "The following page either does not exist or has been moved. Either return home or check the URL you entered."
          }
        </p>
        <div className={"flex justify-center items-center gap-2"}>
          <Button color={"secondary"} onClick={handleBack}>
            {"Back"}
          </Button>
          <Button href={"/"}>{"Home"}</Button>
        </div>
      </div>
    </div>
  );
};

export default NotFound;
