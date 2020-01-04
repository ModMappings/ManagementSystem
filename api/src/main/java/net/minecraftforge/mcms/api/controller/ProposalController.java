package net.minecraftforge.mcms.api.controller;

import net.minecraftforge.mcms.api.model.Proposal;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import java.security.Principal;

@RestController
public class ProposalController {
    @PostMapping(path = "/api/1/proposal")
    public Proposal createProposal(Principal principal, @RequestBody Proposal proposal) {
        return proposal;
    }

    @GetMapping(path = "/api/1/proposal")
    public Proposal getProposal(Principal principal) {
        return new Proposal();
    }
}
