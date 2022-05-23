library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Puerto is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Enable : in  STD_LOGIC;
			  puerto: inout std_logic_Vector (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  clk:in std_logic;
           WE : in  STD_LOGIC);
end  puerto;

architecture Behavioral of  Puerto is

begin
process (clk) is
variable registro:std_logic_vector(31 downto 0):=x"00000000";
begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
		 registro:=x"00000000" or datos;
	 else
	    datos<=registro;
	 end if;


else
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
  puerto<=registro;
end if;
end if;
end process;

end Behavioral;

