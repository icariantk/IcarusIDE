library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity TecladoPS2 is
    Port ( Datos: inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
	   Enable: in  STD_LOGIC;
	   clk:in std_logic;
           WE:in  STD_LOGIC;
           Clk_PS2:in  STD_LOGIC;
           Data_PS2:in  STD_LOGIC
          );
end TecladoPS2 ;

architecture Behavioral of TecladoPS2  is
shared variable registro:std_logic_Vector(31 downto 0);

begin
process (clk_ps2) is
variable contador:integer range 0 to 15:=0;
variable registro2:std_logic_Vector(43 downto 0);
variable estado:integer range 0 to 3:=0;

begin
if clk_ps2'event and clk_ps2='0' then

 registro2:=data_ps2&registro2(43 downto 1);
 contador:=contador+1;
 if contador=11 and estado=1 then
	registro:=registro2(41 downto 34)&registro(31 downto 8);
	estado:=0;
 end if;
 if registro2(41 downto 34)=x"F0" then
 estado:=1;
 end if;
 if contador=11 then
 contador:=0;
 end if;

end if;

end process;

process (clk) is
begin

if clk'event and clk='1' then
if enable='1' then
    if we='0' then
	    datos<=registro;

	 end if;
else
datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;
end process;

end Behavioral;

