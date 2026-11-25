// Resources
import type { Children } from "@servion/utility";
import type { ReactNode } from "react";

// Components
import Card from "./card";
import Callout from "./callout";
import { Button } from "../base/buttons/button";

/** The properties of the card form component. */
export interface CardFormProperties extends Children {
  title: string;
  description: string;
  action: CardFormAction;
  notice?: CardFormNotice;
  pending?: boolean;
  error?: string;
}

/** A simple object with text. */
export interface CardFormText {
  text: string;
}

/** The action data of a card form. */
export interface CardFormAction extends CardFormText {
  accent?: ReactNode;
}

/** A card form notice. */
export interface CardFormNotice extends CardFormText {
  url?: string;
}

/** A reusable card form that contains information in a clean card for the form. */
const CardForm = ({
  children,
  title,
  description,
  notice,
  action,
  pending,
  error,
}: CardFormProperties) => (
  <Card className={"prose"}>
    <h1>{title}</h1>
    {error && <Callout type={"error"}>{error}</Callout>}
    <p>{description}</p>
    <section className={"mb-3 flex flex-col gap-2"}>{children}</section>
    <div className={"flex w-full justify-between items-center"}>
      {notice &&
        (notice.url ? (
          <Button color={"link-color"} href={notice.url}>
            {notice.text}
          </Button>
        ) : (
          <p>{notice.text}</p>
        ))}
      <div className={"flex gap-2 ml-auto"}>
        {action.accent}
        <Button type={"submit"} isLoading={pending}>
          {action.text}
        </Button>
      </div>
    </div>
  </Card>
);

export default CardForm;
