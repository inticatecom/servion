// Components
import { Select } from "@/components/base/select/select";

// Hooks
import useLocale from "@/hooks/use-locale";

// Constants
const YEAR = new Date().getFullYear();

/** The application's base footer. */
const Footer = () => {
  // Hooks
  const { locale } = useLocale();

  return (
    <footer className={"grid grid-cols-3 items-center p-3 w-full absolute bottom-0 mx-auto"}>
      <Select className={"justify-self-start"} value={locale} placeholder={"Language"}>
        <Select.Item id={"en"}>{"EN"}</Select.Item>
      </Select>
      <section className={"prose justify-self-center"}>
        <p>{`© ${YEAR} - ${YEAR + 1} Servion`}</p>
      </section>
    </footer>
  );
};

export default Footer;
