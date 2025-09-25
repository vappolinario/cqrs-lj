{ pkgs ? import <nixpkgs> { config = { allowUnfree = true; }; } }:

pkgs.mkShellNoCC {
  buildInputs = [
    (pkgs.python3.withPackages (ps: with ps; [ virtualenv pip setuptools wheel ]))
     pkgs.playwright
     pkgs.playwright-driver.browsers
  ];

  # set LD_LIBRARY_PATH environment variable to avoid error. see https://discourse.nixos.org/t/how-to-solve-libstdc-not-found-in-shell-nix/25458
  LD_LIBRARY_PATH = "${pkgs.stdenv.cc.cc.lib}/lib:${pkgs.zlib}/lib";

  shellHook = ''
    # create virtualenv if not exist
    if [ ! -d .venv ]; then
      virtualenv .venv
    fi
    # activate virtualenv
    source .venv/bin/activate
    # install aider-chat into virtualenv and upgrade it
    pip install --upgrade aider-chat --prefix=$PWD/.venv
    # export PATH to virtualenv bins
    export PATH=$PWD/.venv/bin:$PATH
    export LC_ALL="C.UTF-8"

      export PLAYWRIGHT_BROWSERS_PATH=${pkgs.playwright-driver.browsers}
      export PLAYWRIGHT_SKIP_VALIDATE_HOST_REQUIREMENTS=true
  '';
  exitHook = ''
    deactivate
  '';
}
