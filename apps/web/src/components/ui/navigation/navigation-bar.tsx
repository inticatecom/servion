// Resources
import type { JSX } from "react";
import type { NavigationMenuItem } from "@/components/ui/navigation/navigation-menu";

// Components
import AccountMenu from "@/components/ui/navigation/account-menu";

/** Placeholder account destinations until profile, security, and sign-out actions are connected. */
const accountItems: NavigationMenuItem[] = [
  { href: "#profile", label: "My profile" },
  { href: "#security", label: "Account security" },
  { href: "#sign-out", label: "Sign out" },
];

/** A destination displayed in the primary navigation. */
export interface NavigationLink {
  href: string;
  label: string;
}

/** The properties of the primary navigation bar. */
interface NavigationBarProps {
  activeHref: string;
  links: NavigationLink[];
}

/** A compact, reusable navigation bar with horizontally scrollable links. */
const NavigationBar = ({ activeHref, links }: NavigationBarProps): JSX.Element => (
  <header className={"flex items-center justify-between gap-4 border-b border-neutral-800"}>
    <nav aria-label={"Main navigation"} className={"flex min-w-0 gap-4 overflow-x-auto sm:gap-6"}>
      {links.map((link) => (
        <a
          key={link.href}
          href={link.href}
          aria-current={link.href === activeHref ? "page" : undefined}
          className={`whitespace-nowrap border-b-2 px-0 py-3 text-sm transition focus-visible:outline-2 focus-visible:outline-white ${link.href === activeHref ? "border-white text-white" : "border-transparent text-neutral-500 hover:text-white"}`}
        >
          {link.label}
        </a>
      ))}
    </nav>
    <div className={"shrink-0"}>
      <AccountMenu items={accountItems} />
    </div>
  </header>
);

export default NavigationBar;
