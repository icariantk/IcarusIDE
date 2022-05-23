
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_LOGIC_unsigned.ALL;

entity Memorias is Port(
	WE: in  STD_LOGIC;
	Enable:in std_logic;
	CLK: in  STD_LOGIC;
	DIRECCION: in  STD_LOGIC_VECTOR (8 downto 0);	Datos: inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
end Memorias;
architecture Behavioral of Memorias is
type arr is array(0 to 511) of std_logic_vector(31 downto 0);--FFFFFE00
signal dats:arr:=(0=>x"FFFFFDE3",1=>x"FFFFFDE5",2=>x"FFFFFDE5",3=>x"FFFFFDE5",4=>x"FFFFFDE5",5=>x"FFFFFDE1",6=>x"FFFFFE08",8=>x"FFFFFE02",others=>x"00000000");
begin


process (clk) is
begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
	    dats(conv_integer(direccion))<=datos;
	 else
	    datos<=dats(conv_integer(direccion));
	 end if;
else
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;


end process;
end Behavioral;