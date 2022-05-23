library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity Divisor is
    Port ( CLK_in : in  STD_LOGIC;
           Clk_out : inout  STD_LOGIC:='1');
end Divisor;

architecture Behavioral of Divisor is

begin
process (clk_in) is
variable alentador:integer range 0 to 2047:=0;
begin

if clk_in'event and clk_in='1' then
alentador:=alentador+1;
if alentador=512 then
clk_out<=not clk_out;
alentador:=0;
end if;
end if;
end process;

end Behavioral;

