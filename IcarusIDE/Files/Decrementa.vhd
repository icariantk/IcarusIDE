library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_logic_unsigned.ALL;

entity decrementa is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Enable : in  STD_LOGIC;
			  clk:in std_logic;
           WE : in  STD_LOGIC);
end  decrementa;

architecture Behavioral of  decrementa is

begin
process (clk) is
variable registro:std_logic_vector(31 downto 0):=x"00000000";
begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
	    registro:=datos;
	 else
	    datos<=registro-x"00000001";
	 end if;
 


else
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;
end process;

end Behavioral;

