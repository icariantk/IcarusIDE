library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity SieteSegmentos is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Enable : in  STD_LOGIC;
			  segmentos:out std_logic_vector(7 downto 0);
			  anodos:inout std_logic_Vector(3 downto 0):="1110";
			  clk:in std_logic;
			  clk_fast:in std_logic;
           WE : in  STD_LOGIC);
end  SieteSegmentos;

architecture Behavioral of  SieteSegmentos is

shared variable registro2:std_logic_vector(31 downto 0):=x"00000000";

begin

process (clk_fast) is
variable mitad:std_logic:='0';
variable registro:std_logic_vector(15 downto 0):=x"0000";
variable estado:integer range 0 to 3:=0;
variable delay2:integer range 0 to 49807360:=0;
variable delay:integer range 0 to 262144;
begin
if clk_fast'event and clk_fast='1' then
delay:=delay+1;
delay2:=delay2+1;
if delay=262144 then
	registro:=registro(3 downto 0)&registro(15 downto 4);
	anodos<=anodos(2 downto 0)&anodos(3);
	delay:=0;
end if;

if delay2=49807360 then
	if mitad='0' then
	   registro:=registro2(15 downto 0);
	else
	   registro:=registro2(31 downto 16);
	end if;
	mitad:=not mitad;
	segmentos(7)<= mitad;
	delay2:=0;
	delay:=0;
	anodos<="1110";
end if;

	
if registro(3 downto 0) = x"0" then
segmentos(6 downto 0)<="1000000";

end if;
if registro(3 downto 0) = x"1" then
segmentos(6 downto 0)<= "1111001";
end if;
if registro(3 downto 0) = x"2" then
segmentos(6 downto 0)<= "0100100";
end if;
if registro(3 downto 0) = x"3" then
segmentos(6 downto 0)<="0110000";
end if;
if registro(3 downto 0) = x"4" then
segmentos(6 downto 0)<="0011001";
end if;
if registro(3 downto 0) = x"5" then
segmentos(6 downto 0)<="0010010";
end if;
if registro(3 downto 0) = x"6" then
segmentos(6 downto 0)<="0000010";
end if;
if registro(3 downto 0) = x"7" then
segmentos(6 downto 0)<="1111000";
end if;
if registro(3 downto 0) = x"8" then
segmentos(6 downto 0)<="0000000";
end if;
if registro(3 downto 0) = x"9" then
segmentos(6 downto 0)<="0011000";
end if;
if registro(3 downto 0) = x"A" then
segmentos(6 downto 0)<="0001000";
end if;
if registro(3 downto 0) = x"B" then
segmentos(6 downto 0)<="0000011";
end if;
if registro(3 downto 0) = x"C" then
segmentos(6 downto 0)<="1000110";
end if;
if registro(3 downto 0) = x"D" then
segmentos(6 downto 0)<="0100001";
end if;
if registro(3 downto 0) = x"E" then
segmentos(6 downto 0)<="0000110";
end if;
if registro(3 downto 0) = x"F" then
segmentos(6 downto 0)<="0001110";
end if;

end if;

end process;

process (clk) is
begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
		 registro2:=x"00000000" or datos;
	 else
	    datos<=registro2;
	 end if;
else
datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;
end process;

end Behavioral;

