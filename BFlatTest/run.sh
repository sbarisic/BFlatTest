#!/usr/bin/env bash
qemu-system-x86_64 \
  -machine q35 \
  -m 2G \
  -smp cpus=4,sockets=1,cores=4,threads=1 \
  -drive if=pflash,format=raw,readonly=on,file=/usr/share/OVMF/OVMF_CODE_4M.fd \
  -drive if=pflash,format=raw,file=OVMF_VARS.fd \
  -drive file=fat:rw:.,format=raw
  