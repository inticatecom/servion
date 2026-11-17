// Resources
import type { JSX, ReactNode } from "react";

// Components
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger
} from "@/components/ui/primitives/dropdown-menu";
import { Button } from "@/components/ui/primitives/button";

/** An available destination or action in a navigation menu. */
export interface NavigationMenuItem {
  label: string;
  href?: string;
  onSelect?: () => void;
}

/** The properties of a reusable navigation menu. */
export interface NavigationMenuProps {
  label: string;
  children: ReactNode;
  items: NavigationMenuItem[];
}

/** An accessible dropdown for navigation destinations and contextual actions. */
const NavigationMenu = ({ label, children, items }: NavigationMenuProps): JSX.Element => (
  <DropdownMenu>
    <DropdownMenuTrigger asChild>
      <Button variant={"ghost"} size={"icon"} aria-label={label} className={"size-9 text-neutral-400 hover:text-white"}>
        {children}
      </Button>
    </DropdownMenuTrigger>
    <DropdownMenuContent align={"end"} sideOffset={8} className={"min-w-44 rounded-lg"}>
      {items.map(item =>
        item.href ? (
          <DropdownMenuItem key={item.label} asChild>
            <a href={item.href}>{item.label}</a>
          </DropdownMenuItem>
        ) : (
          <DropdownMenuItem
            key={item.label}
            onSelect={() => {
              if (item.onSelect) {
                item.onSelect();
              }
            }}
          >
            {item.label}
          </DropdownMenuItem>
        )
      )}
    </DropdownMenuContent>
  </DropdownMenu>
);

export default NavigationMenu;
