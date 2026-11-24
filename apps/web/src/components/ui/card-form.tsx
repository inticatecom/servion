// Resources
import type { Children } from "@servion/utility";
import type { ReactNode } from "react";

// Components
import Card from "./card";
import Callout from "./callout";
import { Button } from "../base/buttons/button";

export interface CardFormProperties extends Children {
  title: string;
  description: string;
  action: CardFormAction;
  pending?: boolean;
  error?: string;
}

export interface CardFormAction {
  text: string;
  accent?: ReactNode;
}

const CardForm = ({
  children,
  title,
  description,
  action,
  pending,
  error,
}: CardFormProperties) => (
  <Card className={"prose"}>
    <h1>{title}</h1>
    {error && <Callout type={"error"}>{error}</Callout>}
    <p>{description}</p>
    <section className={"mb-3 flex flex-col gap-2"}>{children}</section>
    <div className={"flex w-full justify-between"}>
      <div className={"flex gap-2"}>
        <Button type={"submit"} isLoading={pending}>
          {action.text}
        </Button>
      </div>
    </div>
  </Card>
);

export default CardForm;
