library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity desplazarDer is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Enable : in  STD_LOGIC;
			  clk:in std_logic;
           WE : in  STD_LOGIC);
end  desplazarDer;

architecture Behavioral of  desplazarDer is

begin
process (clk) is
variable registroB:std_logic_vector(31 downto 0):=x"00000000";
begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
	    registroB:=datos;
	 else
	    datos<='0'&registroB(31 downto 1);
	 end if;
 


else
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;
end process;

end Behavioral;