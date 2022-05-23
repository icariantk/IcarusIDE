library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.std_logic_unsigned.ALL;
library UNISIM;
use UNISIM.VComponents.all;


entity PC is
generic(
	primeradireccion: std_logic_vector(31 downto 0):=x"00000000"
);
   Port ( 
	enable : in STD_LOGIC;
	rst:in std_logic;
   Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
   clk:in std_logic;
   WE : in  STD_LOGIC);
end PC;


architecture Behavioral of PC is
      
   begin
    process (clk,rst) is
		variable PC:std_logic_vector(31 downto 0):=primeradireccion;		
		variable estado:integer range 0 to 4:=0;
      begin
		if rst='0' then
			if clk'event and clk='1' then
				if enable='1' then
				
						if we='1' then
							pc:=datos;
						else
							estado:=estado+1;
							datos<=PC;
						end if;
						if estado=2 then
							pc:=pc+x"00000002";
							estado:=0;
						end if;
				else
					datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
				end if;
			end if;
		else
			pc:=primeradireccion;
		end if;
    end process;
  end Behavioral;