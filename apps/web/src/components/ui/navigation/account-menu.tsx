// Resources
import type { JSX } from "react";

// Components
import { UserRound } from "lucide-react";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/primitives/avatar";
import NavigationMenu, { type NavigationMenuItem } from "@/components/ui/navigation/navigation-menu";

/** The properties of the account dropdown. */
interface AccountMenuProps {
  items: NavigationMenuItem[];
  avatarUrl?: string;
  name?: string;
}

/** A profile avatar with account navigation and actions. */
const AccountMenu = ({ items, avatarUrl, name = "Account" }: AccountMenuProps): JSX.Element => (
  <NavigationMenu label={`${name} menu`} items={items}>
    <Avatar className={"size-8 border border-neutral-700"}>
      {avatarUrl && <AvatarImage src={avatarUrl} alt={name} />}
      <AvatarFallback className={"bg-neutral-800 text-neutral-300"}>
        <UserRound size={18} aria-hidden />
      </AvatarFallback>
    </Avatar>
  </NavigationMenu>
);

export default AccountMenu;
